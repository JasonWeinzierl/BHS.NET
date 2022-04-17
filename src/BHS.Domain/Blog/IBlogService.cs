using BHS.Contracts.Blog;

namespace BHS.Domain.Blog
{
    public interface IBlogService
    {
        Task<Post?> GetPost(string slug);
        Task<IReadOnlyCollection<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to);
        Task<IReadOnlyCollection<CategorySummary>> GetCategories();
        Task<CategoryPosts?> GetCategory(string slug);
        Task<IReadOnlyCollection<PostPreview>?> GetPostsByAuthor(string username);
    }
}
