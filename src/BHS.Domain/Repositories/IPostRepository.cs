using BHS.Contracts.Blog;

namespace BHS.Domain.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetBySlug(string slug);
    }
}
