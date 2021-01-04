using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PostPreviewRepository : IPostPreviewRepository
    {
        protected IQuerier Q { get; }

        public PostPreviewRepository(IQuerier querier)
        {
            Q = querier;
        }

        public Task<IEnumerable<PostPreview>> Search(string text, DateTimeOffset? from, DateTimeOffset? to)
        {
            return Q.QueryAsync<PostPreview>(Constants.bhsConnectionStringName, "blog.PostPreview_Search", new
            {
                searchText = text,
                fromDate = from,
                toDate = to
            });
        }

        public Task<IEnumerable<PostPreview>> GetByCategorySlug(string categorySlug)
        {
            return Q.QueryAsync<PostPreview>(Constants.bhsConnectionStringName, "blog.PostPreview_GetByCategorySlug", new { categorySlug });
        }

        public Task<IEnumerable<PostPreview>> GetByAuthorId(int authorId)
        {
            return Q.QueryAsync<PostPreview>(Constants.bhsConnectionStringName, "blog.PostPreview_GetByAuthorId", new { authorId });
        }
    }
}
