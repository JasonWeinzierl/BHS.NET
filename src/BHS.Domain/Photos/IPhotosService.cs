using BHS.Contracts.Photos;

namespace BHS.Domain.Photos
{
    public interface IPhotosService
    {
        Task<IReadOnlyCollection<Album>> GetAlbums();
        Task<AlbumPhotos?> GetAlbum(string slug);
        Task<Photo?> GetPhoto(int id);
    }
}
