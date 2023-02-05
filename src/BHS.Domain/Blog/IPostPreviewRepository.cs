using BHS.Contracts.Blog;

namespace BHS.Domain.Blog;

public interface IPostPreviewRepository
{
    Task<IReadOnlyCollection<PostPreview>> GetByAuthorUsername(string username, CancellationToken cancellationToken = default);

    Task<CategoryPosts?> GetCategoryPosts(string categorySlug, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken = default);
}
