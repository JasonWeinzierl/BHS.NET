using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album> GetById(int id);
        IAsyncEnumerable<Album> GetAll(bool doIncludeHidden = false);
    }
}
