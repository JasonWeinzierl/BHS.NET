using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Model.DataAccess;
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

        public async Task<IEnumerable<CategorySummary>> GetAll()
        {
            var results = await E.QueryAsync<CategorySummaryDTO>(Constants.bhsConnectionStringName, "blog.CategorySummary_GetAll");
            return results.Select(r => r.ToDomainModel());
        }

        public Task<Category?> GetBySlug(string slug)
        {
            return E.QuerySingleOrDefaultAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", new { slug });
        }
    }
}
