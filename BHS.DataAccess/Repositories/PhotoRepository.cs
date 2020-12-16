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
        public PhotoRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<Photo> GetById(int id)
        {
            return await ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Photo_GetById", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@id", id, DbType.Int32));
            }, GetPhoto).SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<Photo> GetByAlbumId(int albumId)
        {
            return ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Photo_GetByAlbumId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@albumId", albumId, DbType.Int32));
            }, GetPhoto);
        }

        private static Photo GetPhoto(IDataRecord dr)
        {
            return new Photo(
                ToInt(dr["id"]),
                ToString(dr["Name"]),
                ToUri(dr["ImagePath"]),
                ToBool(dr["IsVisible"]),
                ToDateTimeOffset(dr["DatePosted"]),
                ToNullableInt(dr["AuthorId"])
                );
        }
    }
}
