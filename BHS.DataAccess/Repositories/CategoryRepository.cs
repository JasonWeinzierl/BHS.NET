using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Collections.Generic;
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

        public Task<IEnumerable<Category>> GetAll()
        {
            return E.QueryAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetAll");
        }

        public Task<Category?> GetBySlug(string slug)
        {
            return E.QuerySingleOrDefaultAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", new { slug });
        }
    }
}
