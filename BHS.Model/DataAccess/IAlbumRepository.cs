using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IAlbumRepository
    {
        Task<Album?> GetById(int id);
        Task<IEnumerable<Album>> GetAll();
    }
}
