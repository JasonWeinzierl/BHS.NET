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

        public async Task<Category> GetById(int id)
        {
            return (await ExecuteReaderAsync<List<Category>>(Constants.bhsConnectionStringName, "blog.Category_GetById", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@id", id, DbType.Int32));
            }, FillCategories)).FirstOrDefault();
        }

        public async Task<IEnumerable<Category>> GetByPostId(int postId)
        {
            return await ExecuteReaderAsync<List<Category>>(Constants.bhsConnectionStringName, "blog.Category_GetByPostId", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@postId", postId, DbType.Int32));
            }, FillCategories);
        }

        private void FillCategories(IDataReader dr, ref List<Category> models)
        {
            var model = new Category(
                ToInt(dr["Id"]),
                ToString(dr["Name"]),
                ToBool(dr["IsVisible"])
                );
            models.Add(model);
        }
    }
}
