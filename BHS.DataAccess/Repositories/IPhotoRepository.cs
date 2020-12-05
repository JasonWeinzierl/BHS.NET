using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IPhotoRepository
    {
        Task<Photo> GetById(int id);
        Task<IEnumerable<Photo>> GetByAlbumId(int albumId);
    }
}
