using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Domain.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
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
            var results = await E.QueryAsync<CategorySummaryDto>(Constants.bhsConnectionStringName, "blog.CategorySummary_GetAll");
            return results.Select(r => r.ToDomainModel()).ToList();
        }

        public Task<Category?> GetBySlug(string slug)
        {
            return E.QuerySingleOrDefaultAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", new { slug });
        }
    }
}
