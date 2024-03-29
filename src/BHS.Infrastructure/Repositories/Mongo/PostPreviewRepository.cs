﻿using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class PostPreviewRepository : IPostPreviewRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly TimeProvider _timeProvider;

    public PostPreviewRepository(IMongoClient mongoClient, TimeProvider timeProvider)
    {
        _mongoClient = mongoClient;
        _timeProvider = timeProvider;
    }

    public async Task<IReadOnlyCollection<PostPreview>> GetByAuthorUsername(string username, CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .GetCurrentPostSnapshotDtos(_timeProvider.GetUtcNow())
            .Match(x => x.LatestRevision.Author!.Username == username)
            .GetPreviews()
            .ToListAsync(cancellationToken);

    public async Task<CategoryPosts?> GetCategoryPosts(string categorySlug, CancellationToken cancellationToken = default)
    {
        var posts = await _mongoClient.GetBhsCollection<PostDto>("posts")
                .Aggregate()
                .GetCurrentPostSnapshotDtos(_timeProvider.GetUtcNow())
                .Match(x => x.Categories.Any(y => y.Slug == categorySlug))
                .GetPreviews()
                .ToListAsync(cancellationToken);
        if (!posts.Any()) return null;

        string categoryName = posts.First().Categories.First(x => x.Slug == categorySlug).Name;
        return new CategoryPosts(categorySlug, categoryName, posts);
    }

    public async Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .GetCurrentPostSnapshotDtos(_timeProvider.GetUtcNow())
            .Match(x => (from == null || x.LatestPublication.DatePublished >= from) && (to == null || x.LatestPublication.DatePublished < to))
            .GetPreviews(text)
            .ToListAsync(cancellationToken);
}
