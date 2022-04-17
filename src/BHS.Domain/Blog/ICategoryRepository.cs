using BHS.Contracts.Blog;

namespace BHS.Domain.Blog
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyCollection<CategorySummary>> GetAll();
        Task<Category?> GetBySlug(string slug);
    }
}
