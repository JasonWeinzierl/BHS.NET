using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<Album> GetById(int id)
        {
            return await Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Album_GetById", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@id", id, DbType.Int32));
            }, GetAlbum).SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<Album> GetAll(bool doIncludeHidden = false)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "photos.Album_GetAll", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@doIncludeHidden", doIncludeHidden, DbType.Boolean));
            }, GetAlbum);
        }

        private static Album GetAlbum(IDataRecord dr)
        {
            return new Album(
                dr.CastInt("Id"),
                dr.CastString("Name"),
                dr.CastString("Description"),
                dr.CastNullableInt("BannerPhotoId"),
                dr.CastNullableInt("BlogPostId"),
                dr.CastBool("IsVisible"),
                dr.CastDateTimeOffset("DateUpdated"),
                dr.CastNullableInt("AuthorId")
                );
        }
    }
}
