using BHS.Contracts.Blog;

namespace BHS.Domain.Blog;

public interface IPostPreviewRepository
{
    [Obsolete("Use IPostPreviewRepositoryWithAuthorUserName.GetByAuthorUserName instead.")]
    Task<IReadOnlyCollection<PostPreview>> GetByAuthorId(int authorId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PostPreview>> GetByCategorySlug(string categorySlug, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken = default);
}

public interface IPostPreviewRepositoryWithAuthorUsername : IPostPreviewRepository
{
    Task<IReadOnlyCollection<PostPreview>> GetByAuthorUsername(string username, CancellationToken cancellationToken = default);
}
