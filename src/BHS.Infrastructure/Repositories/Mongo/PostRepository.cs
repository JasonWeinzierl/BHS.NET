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
}
