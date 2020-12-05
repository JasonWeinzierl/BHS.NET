using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class AlbumRepository : SprocRepositoryBase
        , IAlbumRepository
    {
        public AlbumRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<Album> GetById(int id)
        {
            return (await ExecuteReaderAsync<List<Album>>(Constants.bhsConnectionStringName, "photos.Album_GetById", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@id", id, DbType.Int32));
            }, FillAlbums)).FirstOrDefault();
        }

        public async Task<IEnumerable<Album>> GetAll(bool doIncludeHidden = false)
        {
            return await ExecuteReaderAsync<List<Album>>(Constants.bhsConnectionStringName, "photos.Album_GetAll", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@doIncludeHidden", doIncludeHidden, DbType.Boolean));
            }, FillAlbums);
        }

        private void FillAlbums(IDataReader dr, ref List<Album> models)
        {
            var model = new Album(
                ToInt(dr["Id"]),
                ToString(dr["Name"]),
                ToString(dr["Description"]),
                ToNullableInt(dr["BannerPhotoId"]),
                ToNullableInt(dr["BlogPostId"]),
                ToBool(dr["IsVisible"]),
                ToDateTimeOffset(dr["DateUpdated"]),
                ToNullableInt(dr["AuthorId"])
                );
            models.Add(model);
        }
    }
}
