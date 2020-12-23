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
            return await ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.Category_GetBySlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@slug", slug, DbType.Int32));
            }, GetCategory).SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<Category> GetByPostSlug(string postSlug)
        {
            return ExecuteReaderAsync(Constants.bhsConnectionStringName, "blog.Category_GetByPostSlug", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@postSlug", postSlug, DbType.Int32));
            }, GetCategory);
        }

        private static Category GetCategory(IDataRecord dr)
        {
            return new Category(
                dr.CastString("Slug"),
                dr.CastString("Name")
                );
        }
    }
}
