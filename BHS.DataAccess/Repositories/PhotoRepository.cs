using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PhotoRepository : SprocRepositoryBase
        , IPhotoRepository
    {
        public PhotoRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<Photo> GetById(int id)
        {
            return (await ExecuteReaderAsync<List<Photo>>(Constants.bhsConnectionStringName, "photos.Photo_GetById", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@id", id, DbType.Int32));
            }, FillPhotos)).FirstOrDefault();
        }

        public async Task<IEnumerable<Photo>> GetByAlbumId(int albumId)
        {
            return await ExecuteReaderAsync<List<Photo>>(Constants.bhsConnectionStringName, "photos.Photo_GetByAlbumId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@albumId", albumId, DbType.Int32));
            }, FillPhotos);
        }

        private void FillPhotos(IDataReader dr, ref List<Photo> models)
        {
            Uri.TryCreate(ToString(dr["ImagePath"]), UriKind.RelativeOrAbsolute, out Uri imagePath);

            var model = new Photo(
                ToInt(dr["id"]),
                ToString(dr["Name"]),
                imagePath,
                ToBool(dr["IsVisible"]),
                ToDateTimeOffset(dr["DatePosted"]),
                ToNullableInt(dr["AuthorId"])
                );
            models.Add(model);
        }
    }
}
