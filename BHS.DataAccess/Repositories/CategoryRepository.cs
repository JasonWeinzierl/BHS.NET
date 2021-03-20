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

        public async Task<IEnumerable<Category>> GetAll()
        {
            var results = await E.QueryAsync<CategoryDTO>(Constants.bhsConnectionStringName, "blog.Category_GetAll");
            return results.Select(r => r.ToDomainModel());
        }

        public async Task<CategoryPosts?> GetBySlug(string slug)
        {
            var (categories, posts) = await E.QueryMultipleAsync<CategoryDTO, PostPreviewDTO>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", new { slug });

            return categories.SingleOrDefault()?.ToDomainModel(posts.Select(p => p.ToDomainModel()));
        }
    }
}
