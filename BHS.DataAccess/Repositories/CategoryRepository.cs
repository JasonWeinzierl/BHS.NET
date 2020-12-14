using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class CategoryRepository : SprocRepositoryBase
        , ICategoryRepository
    {
        public CategoryRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<Category> GetBySlug(string slug)
        {
            return (await ExecuteReaderAsync<List<Category>>(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@slug", slug, DbType.Int32));
            }, FillCategories)).FirstOrDefault();
        }

        public async Task<IEnumerable<Category>> GetByPostSlug(string postSlug)
        {
            return await ExecuteReaderAsync<List<Category>>(Constants.bhsConnectionStringName, "blog.Category_GetByPostSlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@postSlug", postSlug, DbType.Int32));
            }, FillCategories);
        }

        private void FillCategories(IDataReader dr, ref List<Category> models)
        {
            var model = new Category(
                ToString(dr["Slug"]),
                ToString(dr["Name"])
                );
            models.Add(model);
        }
    }
}
