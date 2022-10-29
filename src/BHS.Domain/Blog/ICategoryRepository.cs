using BHS.Contracts.Blog;

namespace BHS.Domain.Blog;

public interface ICategoryRepository
{
    Task<IReadOnlyCollection<CategorySummary>> GetAll(CancellationToken cancellationToken = default);
    Task<Category?> GetBySlug(string slug, CancellationToken cancellationToken = default);
}
