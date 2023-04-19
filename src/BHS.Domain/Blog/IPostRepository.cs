using BHS.Contracts.Blog;

namespace BHS.Domain.Blog;

public interface IPostRepository
{
    Task<Post?> GetBySlug(string slug, CancellationToken cancellationToken = default);
    Task<string> Insert(PostRequest request, CancellationToken cancellationToken = default);
    Task<Post?> Update(string slug, PostRequest request, CancellationToken cancellationToken);
}
