using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Model.DataAccess;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Album?> GetById(int id)
        {
            var result = await E.QuerySingleOrDefaultAsync<AlbumDTO>(Constants.bhsConnectionStringName, "photos.Album_GetById", new { id });
            return result?.ToDomainModel();
        }

        public async Task<IEnumerable<Album>> GetAll()
        {
            var results = await E.QueryAsync<AlbumDTO>(Constants.bhsConnectionStringName, "photos.Album_GetAll");
            return results.Select(r => r.ToDomainModel());
        }
    }
}
