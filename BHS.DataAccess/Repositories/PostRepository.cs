using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class PostRepository : IPostRepository
    {
        protected IQuerier Q { get; }

        public PostRepository(IQuerier querier)
        {
            Q = querier;
        }

        public Task<Post> GetBySlug(string slug)
        {
            return Q.QuerySingleOrDefaultAsync<Post>(Constants.bhsConnectionStringName, "blog.Post_GetBySlug", new { slug });
        }
    }
}
