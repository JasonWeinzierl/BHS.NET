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
            var results = await E.QueryAsync<PostPreviewCategoryDto>(Constants.bhsConnectionStringName, "blog.PostPreviewCategory_Search", new
            {
                searchText = text,
                fromDate = from,
                toDate = to
            });
            return results.GroupBy(row => row.Slug)
                .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
                .OrderByDescending(p => p.DatePublished);
        }

        public async Task<IEnumerable<PostPreview>> GetByCategorySlug(string categorySlug)
        {
            var results = await E.QueryAsync<PostPreviewCategoryDto>(Constants.bhsConnectionStringName, "blog.PostPreviewCategory_GetByCategorySlug", new
            {
                categorySlug
            });
            return results.GroupBy(row => row.Slug)
                .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
                .OrderByDescending(p => p.DatePublished);
        }

        public async Task<IEnumerable<PostPreview>> GetByAuthorId(int authorId)
        {
            var results = await E.QueryAsync<PostPreviewCategoryDto>(Constants.bhsConnectionStringName, "blog.PostPreviewCategory_GetByAuthorId", new
            {
                authorId
            });
            return results.GroupBy(row => row.Slug)
                .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
                .OrderByDescending(p => p.DatePublished);
        }
    }
}
