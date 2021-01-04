using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        protected IQuerier Q { get; }

        public CategoryRepository(IQuerier querier)
        {
            Q = querier;
        }

        public Task<Category> GetBySlug(string slug)
        {
            return Q.QuerySingleOrDefaultAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", new { slug });
        }

        public Task<IEnumerable<Category>> GetByPostSlug(string postSlug)
        {
            return Q.QueryAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetByPostSlug", new { postSlug });
        }
    }
}
