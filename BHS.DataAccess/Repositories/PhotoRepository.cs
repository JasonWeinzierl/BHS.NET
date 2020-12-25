using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<Photo> GetById(int id)
        {
            return await Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Photo_GetById", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@id", id, DbType.Int32));
            }, GetPhoto).SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<Photo> GetByAlbumId(int albumId)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Photo_GetByAlbumId", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@albumId", albumId, DbType.Int32));
            }, GetPhoto);
        }

        private static Photo GetPhoto(IDataRecord dr)
        {
            return new Photo(
                dr.CastInt("id"),
                dr.CastString("Name"),
                dr.CastUri("ImageUri"),
                dr.CastBool("IsVisible"),
                dr.CastDateTimeOffset("DatePosted"),
                dr.CastNullableInt("AuthorId")
                );
        }
    }
}
