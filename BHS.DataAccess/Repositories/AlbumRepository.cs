using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        protected IDbExecuter E { get; }

        public AlbumRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public Task<Album?> GetById(int id)
        {
            return E.QuerySingleOrDefaultAsync<Album>(Constants.bhsConnectionStringName, "photos.Album_GetById", new { id });
        }

        public Task<IEnumerable<Album>> GetAll()
        {
            return E.QueryAsync<Album>(Constants.bhsConnectionStringName, "photos.Album_GetAll");
        }
    }
}
