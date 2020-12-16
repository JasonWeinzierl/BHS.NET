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
        public PostRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<Post> GetBySlug(string slug)
        {
            return await ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.Post_GetBySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@slug", slug));
            }, GetPost).SingleOrDefaultAsync();
        }

        private static Post GetPost(IDataRecord dr)
        {
            return new Post(
                ToString(dr["Slug"]),
                ToString(dr["Title"]),
                ToString(dr["ContentMarkdown"]),
                ToUri(dr["FilePath"]),
                ToNullableInt(dr["PhotosAlbumId"]),
                ToInt(dr["AuthorId"]),
                ToDateTimeOffset(dr["DatePublished"]),
                ToDateTimeOffset(dr["DateLastModified"]));
        }
    }
}
