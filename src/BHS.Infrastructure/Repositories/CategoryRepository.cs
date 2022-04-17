using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Models;

namespace BHS.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        protected IDbExecuter E { get; }

        public CategoryRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public async Task<IReadOnlyCollection<CategorySummary>> GetAll()
        {
            var results = await E.ExecuteSprocQuery<CategorySummaryDto>(DbConstants.BhsConnectionStringName, "blog.CategorySummary_GetAll");
            return results.Select(r => r.ToDomainModel()).ToList();
        }

        public Task<Category?> GetBySlug(string slug)
        {
            return E.ExecuteSprocQuerySingleOrDefault<Category>(DbConstants.BhsConnectionStringName, "blog.Category_GetBySlug", new { slug });
        }
    }
}
