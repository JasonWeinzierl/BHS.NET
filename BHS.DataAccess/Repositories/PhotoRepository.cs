using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PhotoRepository : SprocRepositoryBase
        , IPhotoRepository
    {
        public PhotoRepository(IQuerier querier) : base(querier) { }

        public async Task<Photo> GetById(int id)
        {
            return await Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Photo_GetById", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@id", id, DbType.Int32));
            }, GetPhoto).SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<Photo> GetByAlbumId(int albumId)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Photo_GetByAlbumId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@albumId", albumId, DbType.Int32));
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
