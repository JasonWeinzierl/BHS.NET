using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        protected IDbExecuter E { get; }

        public PostRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public Task<Post?> GetBySlug(string slug)
        {
            return E.QuerySingleOrDefaultAsync<Post>(Constants.bhsConnectionStringName, "blog.Post_GetBySlug", new { slug });
        }
    }
}
