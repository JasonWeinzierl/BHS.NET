using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PostRepository : SprocRepositoryBase
        , IPostRepository
    {
        public PostRepository(IQuerier querier) : base(querier) { }

        public async Task<Post> GetBySlug(string slug)
        {
            return await Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.Post_GetBySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@slug", slug));
            }, GetPost).SingleOrDefaultAsync();
        }

        private static Post GetPost(IDataRecord dr)
        {
            return new Post(
                dr.CastString("Slug"),
                dr.CastString("Title"),
                dr.CastString("ContentMarkdown"),
                dr.CastUri("FilePath"),
                dr.CastNullableInt("PhotosAlbumId"),
                dr.CastInt("AuthorId"),
                dr.CastDateTimeOffset("DatePublished"),
                dr.CastDateTimeOffset("DateLastModified"));
        }
    }
}
