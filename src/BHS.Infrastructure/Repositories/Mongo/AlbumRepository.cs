using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class AlbumRepository : IAlbumRepository
{
    private readonly IMongoClient _mongoClient;

    public AlbumRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<IReadOnlyCollection<Album>> GetAll(CancellationToken cancellationToken = default)
    {
        var results = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums")
            .Aggregate()
            .Project(x => new AlbumDto(x.Id, x.Slug, x.Name, x.Description, x.BannerPhoto, x.BlogPostSlug, x.Contributor))
            .ToListAsync(cancellationToken);

        return results.Select(x => x.ToAlbum()).ToList();
    }

    public async Task<AlbumPhotos?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var fb = Builders<AlbumPhotosDto>.Filter;
        var filter = fb.Eq(x => x.Slug, slug);

        var cursor = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums")
            .FindAsync(filter, cancellationToken: cancellationToken);
        var result = await cursor.SingleOrDefaultAsync(cancellationToken);

        return result?.ToAlbumPhotos();
    }
}
