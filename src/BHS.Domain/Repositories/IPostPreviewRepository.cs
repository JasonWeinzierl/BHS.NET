using BHS.Contracts.Blog;

namespace BHS.Domain.Repositories
{
    public interface IPostPreviewRepository
    {
        Task<IReadOnlyCollection<PostPreview>> GetByAuthorId(int authorId);
        Task<IReadOnlyCollection<PostPreview>> GetByCategorySlug(string categorySlug);
        Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to);
    }
}
