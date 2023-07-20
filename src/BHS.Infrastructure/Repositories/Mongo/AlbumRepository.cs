using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
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
            .Project(x => new AlbumDto(x.Slug, x.Name, x.Description, x.BannerPhoto, x.BlogPostSlug, x.AuthorUsername, x.Author ?? null))
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

    public async Task<Photo?> GetPhoto(string albumSlug, string photoId, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(photoId, out var photoObjectId))
            throw new InvalidPhotoIdException("The requested photo id is not formatted correctly. It must consist of 24 hexadecimal digits.");

        var result = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums")
            .Aggregate()
            .Match(x => x.Slug == albumSlug)
            .Project(x => new UnwoundPhotosDto(x.Photos))
            .Unwind<UnwoundPhotosDto, UnwoundPhotoDto>(x => x.Photos)
            .Match(x => x.Photos.Id == photoObjectId)
            .SingleOrDefaultAsync(cancellationToken);
        return result?.Photos.ToPhoto();
    }

    public async Task<AlbumPhotos> UpsertAlbumPhotos(AlbumPhotos albumPhotos, CancellationToken cancellationToken = default)
    {
        var collection = _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums");

        var dto = AlbumPhotosDto.FromAlbumPhotos(albumPhotos);
        var replaceOptions = new ReplaceOptions { IsUpsert = true };

        _ = await collection.ReplaceOneAsync(x => x.Slug == dto.Slug, dto, replaceOptions, cancellationToken);

        return dto.ToAlbumPhotos();
    }
}
