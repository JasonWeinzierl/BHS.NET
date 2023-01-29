using BHS.Contracts;
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

    public async Task BulkUpsert(IEnumerable<AlbumPhotos> albums, IEnumerable<Author> authors, CancellationToken cancellationToken = default)
    {
        var fb = Builders<AlbumPhotosDto>.Filter;

        var authorsDict = authors.ToDictionary(x => x.Id, x => x.DisplayName);
        var models = albums.Select(a => new ReplaceOneModel<AlbumPhotosDto>(fb.Where(x => x.Slug == a.Slug), AlbumPhotosDto.FromAlbumPhotos(a, authorsDict)) { IsUpsert = true });

        _ = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums").BulkWriteAsync(models, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<Album>> GetAll(CancellationToken cancellationToken = default)
    {
        var results = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums")
            .Aggregate()
            .Project(x => new AlbumDto(x.Slug, x.Name, x.Description, x.BannerPhoto, x.BlogPostSlug, x.AuthorUsername))
            .ToListAsync(cancellationToken);

        return results.Select(x => x.ToAlbum()).ToList();
    }

    public async Task<AlbumPhotos?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var cursor = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums")
            .FindAsync(x => x.Slug == slug, cancellationToken: cancellationToken);
        var result = await cursor.SingleOrDefaultAsync(cancellationToken);

        return result?.ToAlbumPhotos();
    }

    public async Task<AlbumPhotos> UpsertAlbumPhotos(AlbumPhotos albumPhotos, IEnumerable<Author> authors, CancellationToken cancellationToken = default)
    {
        var collection = _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums");

        var authorsDict = authors.ToDictionary(x => x.Id, x => x.DisplayName);
        var dto = AlbumPhotosDto.FromAlbumPhotos(albumPhotos, authorsDict);
        var replaceOptions = new ReplaceOptions { IsUpsert = true };

        _ = await collection.ReplaceOneAsync(x => x.Slug == dto.Slug, dto, replaceOptions, cancellationToken);

        return dto.ToAlbumPhotos();
    }
}
