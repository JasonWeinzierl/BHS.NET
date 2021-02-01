using BHS.Contracts.Blog;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IPostRepository
    {
        Task<Post> GetBySlug(string slug);
    }
}
