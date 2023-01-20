using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class PhotoRepository : IPhotoRepository
{
    private readonly IMongoClient _mongoClient;

    public PhotoRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<Photo?> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _mongoClient.GetBhsCollection<AlbumPhotosDto>("albums")
            .Aggregate()
            .Project(x => new UnwoundPhotosDto(x.Photos))
            .Unwind<UnwoundPhotosDto, UnwoundPhotoDto>(x => x.Photos)
            .Match(x => x.Photos.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
        return result?.Photos.ToPhoto();
    }
}
