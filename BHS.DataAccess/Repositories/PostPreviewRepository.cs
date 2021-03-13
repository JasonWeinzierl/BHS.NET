using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PostPreviewRepository : IPostPreviewRepository
    {
        protected IDbExecuter E { get; }

        public PostPreviewRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public Task<IEnumerable<PostPreview>> Search(string text, DateTimeOffset? from, DateTimeOffset? to)
        {
            return E.QueryAsync<PostPreview>(Constants.bhsConnectionStringName, "blog.PostPreview_Search", new
            {
                searchText = text,
                fromDate = from,
                toDate = to
            });
        }

        public Task<IEnumerable<PostPreview>> GetByCategorySlug(string categorySlug)
        {
            return E.QueryAsync<PostPreview>(Constants.bhsConnectionStringName, "blog.PostPreview_GetByCategorySlug", new { categorySlug });
        }

        public Task<IEnumerable<PostPreview>> GetByAuthorId(int authorId)
        {
            return E.QueryAsync<PostPreview>(Constants.bhsConnectionStringName, "blog.PostPreview_GetByAuthorId", new { authorId });
        }
    }
}
