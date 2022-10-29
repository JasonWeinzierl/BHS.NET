using BHS.Contracts.Photos;

namespace BHS.Domain.Photos;

public interface IPhotosService
{
    Task<IReadOnlyCollection<Album>> GetAlbums(CancellationToken cancellationToken = default);
    Task<AlbumPhotos?> GetAlbum(string slug, CancellationToken cancellationToken = default);
    Task<Photo?> GetPhoto(int id, CancellationToken cancellationToken = default);
}
