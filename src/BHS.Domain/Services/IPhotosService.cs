using BHS.Contracts.Photos;

namespace BHS.Domain.Services
{
    public interface IPhotosService
    {
        Task<IReadOnlyCollection<Album>> GetAlbums();
        Task<AlbumPhotos?> GetAlbum(string slug);
        Task<Photo?> GetPhoto(int id);
    }
}
