using BHS.Contracts;
using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class AlbumRepository : IAlbumRepository
{
    private readonly IMongoClient _mongoClient;

    private IMongoCollection<AlbumDto> Collection => _mongoClient.GetDatabase("bhs").GetCollection<AlbumDto>("albums");

    public AlbumRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<IReadOnlyCollection<Album>> GetAll(CancellationToken cancellationToken = default)
    {
        var results = await Collection.Aggregate()
            .Project(x => new
            {
                x.Slug,
                x.Name,
                x.Description,
                x.BannerPhoto,
                x.BlogPostSlug,
                x.Contributor,
                // Exclude Photos array.
            })
            .ToListAsync(cancellationToken);

        return results
            .Select(x => new Album(
                x.Slug,
                x.Name,
                x.Description,
                x.BannerPhoto is null ? null : new Photo(
                    0,
                    x.BannerPhoto.Name,
                    new Uri(x.BannerPhoto.ImagePath),
                    x.BannerPhoto.DatePosted,
                    x.BannerPhoto.Contributor is null ? null : 0,
                    x.BannerPhoto.Description),
                x.BlogPostSlug,
                x.Contributor is null ? null : new Author(
                    0,
                    x.Contributor.UserName,
                    x.Contributor.DisplayName)))
            .ToList();
    }

    public async Task<AlbumPhotos?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var fb = Builders<AlbumDto>.Filter;
        var filter = fb.Eq(x => x.Slug, slug);

        var cursor = await Collection.FindAsync(filter, cancellationToken: cancellationToken);
        var result = await cursor.SingleOrDefaultAsync(cancellationToken);

        return new AlbumPhotos(
            result.Slug,
            result.Name,
            result.Description,
            result.BannerPhoto is null ? null : new Photo(
                0,
                result.BannerPhoto.Name,
                new Uri(result.BannerPhoto.ImagePath),
                result.BannerPhoto.DatePosted,
                result.BannerPhoto.Contributor is null ? null : 0,
                result.BannerPhoto.Description),
            result.BlogPostSlug,
            result.Contributor is null ? null : new Author(
                0,
                result.Contributor.UserName,
                result.Contributor.DisplayName),
            result.Photos.Select(x => new Photo(0, x.Name, new Uri(x.ImagePath), x.DatePosted, 0, x.Description)).ToList());
    }
}
