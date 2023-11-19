using BHS.Contracts.Blog;
using BHS.Domain;
using BHS.Domain.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace BHS.Infrastructure.Repositories.Mongo;

public partial class PostRepository : IPostRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;
    private readonly ISequenceRepository _sequenceRepository;

    public PostRepository(
        IMongoClient mongoClient,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        ISequenceRepository sequenceRepository)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _sequenceRepository = sequenceRepository;
    }

    public async Task<Post?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .Match(x => x.Slug == slug) // TODO: should be case insensitive & elsewhere!
            .GetCurrentPostSnapshotDtos(_dateTimeOffsetProvider.Now())
            .SingleOrDefaultAsync(cancellationToken);
        return result?.ToPost();
    }

    public async Task<string> Insert(PostRequest request, CancellationToken cancellationToken = default)
    {
        var collection = _mongoClient.GetBhsCollection<PostDto>("posts");

        var id = await _sequenceRepository.GetNextValue("posts", cancellationToken);
        var slug = $"{id}-{SlugRegex().Replace(request.Title.Kebaberize(), string.Empty)}";

        var dto = PostDto.New(
            DateTimeOffset.Now,
            slug,
            request.Title,
            request.ContentMarkdown,
            request.FilePath,
            request.PhotosAlbumSlug,
            request.Author,
            request.DatePublished,
            request.Categories);

        await collection.InsertOneAsync(dto, null, cancellationToken);

        return slug;
    }

    [GeneratedRegex("[^a-z0-9-]")]
    private static partial Regex SlugRegex();

    public async Task<Post?> Update(string slug, PostRequest request, CancellationToken cancellationToken)
    {
        var collection = _mongoClient.GetBhsCollection<PostDto>("posts");

        var dtoCursor = await collection.FindAsync(p => p.Slug == slug, cancellationToken: cancellationToken);
        var postDto = await dtoCursor.SingleOrDefaultAsync(cancellationToken); // Get the complete post history.
        var post = await GetBySlug(slug, cancellationToken); // Get the externally visible snapshot.
        if (postDto is null || post is null) return null; // Currently not allowing updates to deleted/unpublished posts.

        var now = DateTimeOffset.Now;

        // TODO: this is domain logic which should be moved out of this project. Refactor and test logic in the Dto's.
        // Want to introduce the example of "simple" vs "advanced" user. Simple == typical CRUD. Advanced == full visibility to immutable timeline.

        var newCategories = new Dictionary<string, PostCategoryHistoryDto>();
        var categoryChanges = new Dictionary<string, PostCategoryChangeDto>();

        var unchangedCategorySlugs = new HashSet<string>();
        foreach (var category in request.Categories)
        {
            var existing = postDto.Categories.FirstOrDefault(c => c.Slug == category.Slug);
            if (existing is null)
            {
                // If category was never added, start a new history.
                newCategories.Add(category.Slug, PostCategoryHistoryDto.New(now, category));
            }
            else if (post.Categories.All(c => c.Slug != category.Slug))
            {
                // Else if category is not externally visible, then it was previously hidden, so enable it.
                categoryChanges.Add(existing.Slug, PostCategoryChangeDto.New(now, true));
            }
            else
            {
                // Else if category is already visible, don't change it.
                bool wasAdded = unchangedCategorySlugs.Add(category.Slug);
                if (!wasAdded)
                    throw new InvalidOperationException($"Duplicate category {category.Slug} found in post {post.Slug}!");
            }
        }

        foreach (var categoryHistory in postDto.Categories)
        {
            // If category was already handled, done.
            if (categoryChanges.ContainsKey(categoryHistory.Slug) || unchangedCategorySlugs.Contains(categoryHistory.Slug))
                continue;

            if (categoryHistory.Changes.OrderBy(c => c.DateChanged).LastOrDefault()?.IsEnabled == false)
            {
                // Else if category is already hidden, don't change it.
                unchangedCategorySlugs.Add(categoryHistory.Slug);
                continue;
            }

            // Else if category was visible but excluded from request, disable it.
            categoryChanges.Add(categoryHistory.Slug, PostCategoryChangeDto.New(now, false));
        }

        PostRevisionDto? newRevision = null;
        KeyValuePair<ObjectId, PostRevisionPublicationDto>? newPublication = null;

        if (request.Title != post.Title
            || request.ContentMarkdown != post.ContentMarkdown
            || request.FilePath != post.FilePath
            || request.PhotosAlbumSlug != post.PhotosAlbumSlug
            || request.Author != post.Author)
        {
            newRevision = PostRevisionDto.New(
                now,
                request.Title,
                request.ContentMarkdown,
                request.FilePath,
                request.PhotosAlbumSlug,
                request.Author,
                request.DatePublished);
        }
        else if (request.DatePublished != post.DatePublished)
        {
            var latestRevision = postDto.Revisions
                .SelectMany(rev => rev.Publications, (rev, pub) => new { Revision = rev, Publication = pub })
                .OrderBy(x => x.Publication.DateCommitted)
                .Last().Revision;

            newPublication = new(latestRevision.Id, PostRevisionPublicationDto.New(request.DatePublished, now));
        }

        var updates = new Dictionary<string, UpdateDefinition<PostDto>>();
        var b = Builders<PostDto>.Update;
        var arrayFilters = new Dictionary<string, ArrayFilterDefinition>(); // We're using a dictionary to pair these up with the relevant update definition.

        if (newCategories.Any())
        {
            updates.Add(
                Guid.NewGuid().ToString(), // Random placeholder.
                b.AddToSetEach(post => post.Categories, newCategories.Values));
        }
        if (categoryChanges.Any())
        {
            foreach (var change in categoryChanges)
            {
                // Use the category slug as the $[<identifier>] position operator's identifier.
                updates.Add(
                    change.Key,
                    // TODO: LINQ3 provides AllMatchingElements extension method for strongly typed field definition. Get rid of manual $[<identifier>] strings.
                    b.Push($"{nameof(PostDto.Categories)}.$[{change.Key}].{nameof(PostCategoryHistoryDto.Changes)}", change.Value));
                // There's no strongly typed array filters available.
                arrayFilters.Add(
                    change.Key,
                    new BsonDocumentArrayFilterDefinition<PostCategoryHistoryDto>(
                        new BsonDocument($"{change.Key}._id", change.Key)));
            }
        }
        if (newRevision is not null)
        {
            updates.Add(
                newRevision.Id.ToString(),
                b.Push(post => post.Revisions, newRevision));
        }
        if (newPublication.HasValue)
        {
            updates.Add(
                newPublication.Value.Key.ToString(),
                // Ditto LINQ3
                b.Push($"{nameof(PostDto.Revisions)}.$[{newPublication.Value.Key}].{nameof(PostRevisionDto.Publications)}", newPublication.Value.Value));
            arrayFilters.Add(
                newPublication.Value.Key.ToString(),
                new BsonDocumentArrayFilterDefinition<PostRevisionDto>(
                    new BsonDocument($"{newPublication.Value.Key}._id", newPublication.Value.Key)));
        }

        if (!updates.Any())
            return post;

        // We can't combine into a single update because of .Categories and .Categories.$[].Changes paths conflicts.
        // We can't use a transaction because standalone servers don't support transactions.
        foreach (var update in updates)
        {
            var updateOptions = new UpdateOptions();
            if (arrayFilters.TryGetValue(update.Key, out var f))
                updateOptions.ArrayFilters = [f];
            _ = await collection.UpdateOneAsync(post => post.Slug == slug, update.Value, updateOptions, cancellationToken);
        }

        return await GetBySlug(slug, cancellationToken); // TODO: reconsider this vs returning true/false.
    }

    public async Task<bool> Delete(string slug, CancellationToken cancellationToken = default)
    {
        var collection = _mongoClient.GetBhsCollection<PostDto>("posts");

        var dtoCursor = await collection.FindAsync(p => p.Slug == slug, cancellationToken: cancellationToken);
        var postDto = await dtoCursor.SingleOrDefaultAsync(cancellationToken); // Get the complete post history.
        var post = await GetBySlug(slug, cancellationToken); // Get the externally visible snapshot.
        if (postDto is null || post is null) return false; // Currently not allowing updates to deleted/unpublished posts.

        var now = DateTimeOffset.Now;

        var updateDefinition = Builders<PostDto>.Update.Push(post => post.Deletions, PostDeletionDto.New(now));

        _ = await collection.UpdateOneAsync(p => p.Slug == slug, updateDefinition, cancellationToken: cancellationToken);

        return true;
    }
}
