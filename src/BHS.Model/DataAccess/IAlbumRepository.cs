using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IAlbumRepository
    {
        Task<AlbumPhotos?> GetBySlug(string slug);
        Task<IReadOnlyCollection<Album>> GetAll();
    }
}
