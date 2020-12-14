using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
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
            return (await ExecuteReaderAsync<List<Post>>(Constants.bhsConnectionStringName, "blog.Post_GetBySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@slug", slug));
            }, FillPosts)).FirstOrDefault();
        }

        public async Task<IEnumerable<Post>> Search(string text, DateTimeOffset? from, DateTimeOffset? to)
        {
            return await ExecuteReaderAsync<List<Post>>(Constants.bhsConnectionStringName, "blog.Post_Search", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@searchText", text));
                cmd.Parameters.Add(CreateParameter(cmd, "@fromDate", from, DbType.DateTimeOffset));
                cmd.Parameters.Add(CreateParameter(cmd, "@toDate", to, DbType.DateTimeOffset));
            }, FillPosts);
        }

        public async Task<IEnumerable<Post>> GetByCategorySlug(string categorySlug)
        {
            return await ExecuteReaderAsync<List<Post>>(Constants.bhsConnectionStringName, "blog.Post_GetByCategorySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@categorySlug", categorySlug));
            }, FillPosts);
        }

        public async Task<IEnumerable<Post>> GetByAuthorId(int authorId)
        {
            return await ExecuteReaderAsync<List<Post>>(Constants.bhsConnectionStringName, "blog.Post_GetByAuthorId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@authorId", authorId));
            }, FillPosts);
        }

        private void FillPosts(IDataReader dr, ref List<Post> models)
        {
            var model = new Post(
                ToString(dr["Slug"]),
                ToString(dr["Title"]),
                ToString(dr["ContentMarkdown"]),
                ToUri(dr["FilePath"]),
                ToNullableInt(dr["PhotosAlbumId"]),
                ToInt(dr["AuthorId"]),
                ToDateTimeOffset(dr["DatePublished"]),
                ToDateTimeOffset(dr["DateLastModified"])
                );
            models.Add(model);
        }
    }
}
