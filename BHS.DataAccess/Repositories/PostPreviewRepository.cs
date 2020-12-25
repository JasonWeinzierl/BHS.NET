using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Data;

namespace BHS.DataAccess.Repositories
{
    public class PostPreviewRepository : IPostPreviewRepository
    {
        protected IQuerier Q { get; }

        public PostPreviewRepository(IQuerier querier)
        {
            Q = querier;
        }

        public IAsyncEnumerable<PostPreview> Search(string text, DateTimeOffset? from, DateTimeOffset? to)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.PostPreview_Search", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@searchText", text));
                cmd.Parameters.Add(cmd.CreateParameter("@fromDate", from, DbType.DateTimeOffset));
                cmd.Parameters.Add(cmd.CreateParameter("@toDate", to, DbType.DateTimeOffset));
            }, GetPostPreview);
        }

        public IAsyncEnumerable<PostPreview> GetByCategorySlug(string categorySlug)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.PostPreview_GetByCategorySlug", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@categorySlug", categorySlug));
            }, GetPostPreview);
        }

        public IAsyncEnumerable<PostPreview> GetByAuthorId(int authorId)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.PostPreview_GetByAuthorId", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@authorId", authorId));
            }, GetPostPreview);
        }

        private static PostPreview GetPostPreview(IDataRecord dr)
        {
            return new PostPreview(
                dr.CastString("Slug"),
                dr.CastString("Title"),
                dr.CastString("ContentPreview"),
                dr.CastInt("AuthorId"),
                dr.CastDateTimeOffset("DatePublished")
                );
        }
    }
}
