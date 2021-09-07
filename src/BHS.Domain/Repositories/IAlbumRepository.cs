using BHS.Contracts.Photos;

namespace BHS.Domain.Repositories
{
    public interface IAlbumRepository
    {
        Task<AlbumPhotos?> GetBySlug(string slug);
        Task<IReadOnlyCollection<Album>> GetAll();
    }
}
