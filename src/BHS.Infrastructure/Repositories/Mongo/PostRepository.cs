using BHS.Contracts.Blog;
using BHS.Domain;
using BHS.Domain.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class PostRepository : IPostRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public PostRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
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
}
