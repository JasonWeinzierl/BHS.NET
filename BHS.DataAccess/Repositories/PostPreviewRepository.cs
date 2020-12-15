using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Data;

namespace BHS.DataAccess.Repositories
{
    public class PostPreviewRepository : SprocRepositoryBase
        , IPostPreviewRepository
    {
        public PostPreviewRepository(IDbConnectionFactory factory) : base(factory) { }

        public IAsyncEnumerable<PostPreview> Search(string text, DateTimeOffset? from, DateTimeOffset? to)
        {
            return ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.PostPreview_Search", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@searchText", text));
                cmd.Parameters.Add(CreateParameter(cmd, "@fromDate", from, DbType.DateTimeOffset));
                cmd.Parameters.Add(CreateParameter(cmd, "@toDate", to, DbType.DateTimeOffset));
            }, GetPostPreview);
        }

        public IAsyncEnumerable<PostPreview> GetByCategorySlug(string categorySlug)
        {
            return ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.PostPreview_GetByCategorySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@categorySlug", categorySlug));
            }, GetPostPreview);
        }

        public IAsyncEnumerable<PostPreview> GetByAuthorId(int authorId)
        {
            return ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.PostPreview_GetByAuthorId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@authorId", authorId));
            }, GetPostPreview);
        }

        private static PostPreview GetPostPreview(IDataRecord dr)
        {
            return new PostPreview(
                ToString(dr["Slug"]),
                ToString(dr["Title"]),
                ToString(dr["ContentPreview"]),
                ToInt(dr["AuthorId"]),
                ToDateTimeOffset(dr["DatePublished"])
                );
        }
    }
}
