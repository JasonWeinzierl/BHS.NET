using BHS.Contracts.Blog;
using BHS.Domain;
using BHS.Domain.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class PostPreviewRepository : IPostPreviewRepository, IPostPreviewRepositoryWithAuthorUsername
{
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public PostPreviewRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public Task<IReadOnlyCollection<PostPreview>> GetByAuthorId(int authorId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("The MongoDB implementation of this repository does not support lookup by author ID.");
    }

    public async Task<IReadOnlyCollection<PostPreview>> GetByAuthorUsername(string username, CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .GetCurrentPostSnapshotDtos(_dateTimeOffsetProvider.Now())
            .Match(x => x.LatestRevision.Author!.Username == username)
            .GetPreviews()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<PostPreview>> GetByCategorySlug(string categorySlug, CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .GetCurrentPostSnapshotDtos(_dateTimeOffsetProvider.Now())
            .Match(x => x.Categories.Any(y => y.Slug == categorySlug))
            .GetPreviews()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .GetCurrentPostSnapshotDtos(_dateTimeOffsetProvider.Now())
            .Match(x => (from == null || x.DateFirstPublished >= from) && (to == null || x.DateFirstPublished < to))
            .GetPreviews(text)
            .ToListAsync(cancellationToken);
}
