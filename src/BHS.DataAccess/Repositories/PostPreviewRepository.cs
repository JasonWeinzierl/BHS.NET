using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Domain.Repositories;

namespace BHS.DataAccess.Repositories
{
    public class PostPreviewRepository : IPostPreviewRepository
    {
        protected IDbExecuter E { get; }

        public PostPreviewRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public async Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to)
        {
            var results = await E.ExecuteSprocQuery<PostPreviewCategoryDto>(DbConstants.BhsConnectionStringName, "blog.PostPreviewCategory_Search", new
            {
                searchText = text,
                fromDate = from,
                toDate = to
            });
            return results.GroupBy(row => row.Slug)
                .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
                .OrderByDescending(p => p.DatePublished)
                .ToList();
        }

        public async Task<IReadOnlyCollection<PostPreview>> GetByCategorySlug(string categorySlug)
        {
            var results = await E.ExecuteSprocQuery<PostPreviewCategoryDto>(DbConstants.BhsConnectionStringName, "blog.PostPreviewCategory_GetByCategorySlug", new
            {
                categorySlug
            });
            return results.GroupBy(row => row.Slug)
                .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
                .OrderByDescending(p => p.DatePublished)
                .ToList();
        }

        public async Task<IReadOnlyCollection<PostPreview>> GetByAuthorId(int authorId)
        {
            var results = await E.ExecuteSprocQuery<PostPreviewCategoryDto>(DbConstants.BhsConnectionStringName, "blog.PostPreviewCategory_GetByAuthorId", new
            {
                authorId
            });
            return results.GroupBy(row => row.Slug)
                .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
                .OrderByDescending(p => p.DatePublished)
                .ToList();
        }
    }
}
