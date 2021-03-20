using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Model.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to)
        {
            var results = await E.QueryAsync<PostPreviewDTO>(Constants.bhsConnectionStringName, "blog.PostPreview_Search", new
            {
                searchText = text,
                fromDate = from,
                toDate = to
            });
            return results.Select(r => r.ToDomainModel());
        }

        public async Task<IEnumerable<PostPreview>> GetByAuthorId(int authorId)
        {
            var results = await E.QueryAsync<PostPreviewDTO>(Constants.bhsConnectionStringName, "blog.PostPreview_GetByAuthorId", new { authorId });
            return results.Select(r => r.ToDomainModel());
        }
    }
}
