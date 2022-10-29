using BHS.Contracts.Blog;

namespace BHS.Domain.Blog;

public interface IBlogService
{
    Task<Post?> GetPost(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CategorySummary>> GetCategories(CancellationToken cancellationToken = default);
    Task<CategoryPosts?> GetCategory(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<PostPreview>?> GetPostsByAuthor(string username, CancellationToken cancellationToken = default);
}
