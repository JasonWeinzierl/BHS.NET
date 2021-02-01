using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        protected IQuerier Q { get; }

        public PhotoRepository(IQuerier querier)
        {
            Q = querier;
        }

        public Task<Photo> GetById(int id)
        {
            return Q.QuerySingleOrDefaultAsync<Photo>(Constants.bhsConnectionStringName, "photos.Photo_GetById", new { id });
        }

        public Task<IEnumerable<Photo>> GetByAlbumId(int albumId)
        {
            return Q.QueryAsync<Photo>(Constants.bhsConnectionStringName, "photos.Photo_GetByAlbumId", new { albumId });
        }
    }
}
