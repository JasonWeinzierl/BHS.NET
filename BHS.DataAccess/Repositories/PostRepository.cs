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

        public async Task<Post> GetById(int id)
        {
            return (await ExecuteReaderAsync<List<Post>>(Constants.bhsConnectionStringName, "blog.Post_GetById", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@id", id, DbType.Int32));
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

        public async Task<IEnumerable<Post>> GetByCategoryId(int categoryId)
        {
            return await ExecuteReaderAsync<List<Post>>(Constants.bhsConnectionStringName, "blog.Post_GetByCategoryId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@categoryId", categoryId));
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
                ToInt(dr["Id"]),
                ToString(dr["Title"]),
                ToString(dr["BodyContent"]),
                ToString(dr["FilePath"]),
                ToNullableInt(dr["PhotosAlbumId"]),
                ToBool(dr["IsVisible"]),
                ToInt(dr["AuthorId"]),
                ToDateTimeOffset(dr["PublishDate"])
                );
            models.Add(model);
        }
    }
}
