using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album> GetById(int id);
        Task<IEnumerable<Album>> GetAll(bool doIncludeHidden = false);
    }
}
