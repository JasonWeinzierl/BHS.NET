using BHS.Contracts.Photos;

namespace BHS.Domain.Photos
{
    public interface IAlbumRepository
    {
        Task<AlbumPhotos?> GetBySlug(string slug);
        Task<IReadOnlyCollection<Album>> GetAll();
    }
}
