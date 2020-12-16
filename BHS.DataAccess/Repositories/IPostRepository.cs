using BHS.Contracts.Blog;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetBySlug(string slug);
    }
}
