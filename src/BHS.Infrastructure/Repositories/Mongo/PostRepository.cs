using BHS.Contracts.Blog;
using BHS.Domain;
using BHS.Domain.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using Humanizer;
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

        var updatedCategories = new List<PostCategoryHistoryDto>();
        foreach (var category in request.Categories)
        {
            var existing = postDto.Categories.FirstOrDefault(c => c.Slug == category.Slug);
            if (existing is null)
            {
                // If category was never added, start a new history.
                updatedCategories.Add(PostCategoryHistoryDto.New(now, category));
            }
            else if (post.Categories.All(c => c.Slug != category.Slug))
            {
                // Else if category is not externally visible, then it was previously hidden, so enable it.
                var changes = existing.Changes.ToList();
                changes.Add(PostCategoryChangeDto.New(now, true));
                updatedCategories.Add(existing with { Changes = changes });
            }
            else
            {
                // Else if category is already visible, don't change it.
                updatedCategories.Add(existing);
            }
        }

        foreach (var categoryHistory in postDto.Categories)
        {
            // If category was already handled, done.
            var updated = updatedCategories.FirstOrDefault(c => c.Slug == categoryHistory.Slug);
            if (updated is not null)
                continue;

            if (categoryHistory.Changes.OrderBy(c => c.DateChanged).LastOrDefault()?.IsEnabled == false)
            {
                // Else if category is already hidden, don't change it.
                updatedCategories.Add(categoryHistory);
                continue;
            }

            // Else if category was visible but excluded from request, disable it.
            var changes = categoryHistory.Changes.ToList();
            changes.Add(PostCategoryChangeDto.New(now, false));
            updatedCategories.Add(categoryHistory with { Changes = changes });
        }

        if (updatedCategories.Any())
        {
            postDto = postDto with { Categories = updatedCategories };
        }

        if (request.Title != post.Title
            || request.ContentMarkdown != post.ContentMarkdown
            || request.FilePath != post.FilePath
            || request.PhotosAlbumSlug != post.PhotosAlbumSlug
            || request.Author != post.Author)
        {
            var newRevision = PostRevisionDto.New(
                now,
                request.Title,
                request.ContentMarkdown,
                request.FilePath,
                request.PhotosAlbumSlug,
                request.Author,
                request.DatePublished);

            var updatedRevisions = postDto.Revisions.Append(newRevision).ToList();

            postDto = postDto with { Revisions = updatedRevisions };
        }
        else if (request.DatePublished != post.DatePublished)
        {
            var latestRevision = postDto.Revisions
                .SelectMany(rev => rev.Publications, (rev, pub) => new { Revision = rev, Publication = pub })
                .OrderBy(x => x.Publication.DateCommitted)
                .Last().Revision;

            var newPublication = PostRevisionPublicationDto.New(request.DatePublished, now);
            var newLatestRevision = latestRevision with { Publications = latestRevision.Publications.Append(newPublication).ToList() };

            var updatedRevisions = postDto.Revisions.ToList();
            updatedRevisions[updatedRevisions.IndexOf(latestRevision)] = newLatestRevision;

            postDto = postDto with { Revisions = updatedRevisions };
        }

        _ = await collection.ReplaceOneAsync(p => p.Slug == slug, postDto, cancellationToken: cancellationToken);

        return await GetBySlug(slug, cancellationToken);
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
