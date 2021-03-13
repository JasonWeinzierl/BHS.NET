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

        public Task<Category> GetBySlug(string slug)
        {
            return E.QuerySingleOrDefaultAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", new { slug });
        }

        public Task<IEnumerable<Category>> GetByPostSlug(string postSlug)
        {
            return E.QueryAsync<Category>(Constants.bhsConnectionStringName, "blog.Category_GetByPostSlug", new { postSlug });
        }
    }
}
