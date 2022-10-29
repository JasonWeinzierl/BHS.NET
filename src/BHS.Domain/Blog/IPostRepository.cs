using BHS.Contracts.Blog;

namespace BHS.Domain.Blog;

public interface IPostRepository
{
    Task<Post?> GetBySlug(string slug, CancellationToken cancellationToken = default);
}
