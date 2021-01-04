using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        protected IQuerier Q { get; }

        public AlbumRepository(IQuerier querier)
        {
            Q = querier;
        }

        public Task<Album> GetById(int id)
        {
            return Q.QuerySingleOrDefaultAsync<Album>(Constants.bhsConnectionStringName, "photos.Album_GetById", new { id });
        }

        public Task<IEnumerable<Album>> GetAll(bool doIncludeHidden = false)
        {
            return Q.QueryAsync<Album>(Constants.bhsConnectionStringName, "photos.Album_GetAll", new { doIncludeHidden });
        }
    }
}
