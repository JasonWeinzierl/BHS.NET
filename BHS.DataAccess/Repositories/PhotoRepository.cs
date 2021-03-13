using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        protected IDbExecuter E { get; }

        public PhotoRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public Task<Photo?> GetById(int id)
        {
            return E.QuerySingleOrDefaultAsync<Photo>(Constants.bhsConnectionStringName, "photos.Photo_GetById", new { id });
        }

        public Task<IEnumerable<Photo>> GetByAlbumId(int albumId)
        {
            return E.QueryAsync<Photo>(Constants.bhsConnectionStringName, "photos.Photo_GetByAlbumId", new { albumId });
        }
    }
}
