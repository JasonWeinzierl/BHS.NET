using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Models;

namespace BHS.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    protected IDbExecuter E { get; }

    public CategoryRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<IReadOnlyCollection<CategorySummary>> GetAll(CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<CategorySummaryDto>(DbConstants.BhsConnectionStringName, "blog.CategorySummary_GetAll", cancellationToken: cancellationToken);
        return results.Select(r => r.ToDomainModel()).ToList();
    }

    public Task<Category?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        return E.ExecuteSprocQuerySingleOrDefault<Category>(DbConstants.BhsConnectionStringName, "blog.Category_GetBySlug", new { slug }, cancellationToken);
    }
}
