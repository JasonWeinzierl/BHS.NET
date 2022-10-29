using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Models;

namespace BHS.Infrastructure.Repositories;

public class PostPreviewRepository : IPostPreviewRepository
{
    protected IDbExecuter E { get; }

    public PostPreviewRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to, CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<PostPreviewCategoryDto>(DbConstants.BhsConnectionStringName, "blog.PostPreviewCategory_Search", new
        {
            searchText = text,
            fromDate = from,
            toDate = to
        }, cancellationToken);
        return results.GroupBy(row => row.Slug)
            .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
            .OrderByDescending(p => p.DatePublished)
            .ToList();
    }

    public async Task<IReadOnlyCollection<PostPreview>> GetByCategorySlug(string categorySlug, CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<PostPreviewCategoryDto>(DbConstants.BhsConnectionStringName, "blog.PostPreviewCategory_GetByCategorySlug", new
        {
            categorySlug
        }, cancellationToken);
        return results.GroupBy(row => row.Slug)
            .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
            .OrderByDescending(p => p.DatePublished)
            .ToList();
    }

    public async Task<IReadOnlyCollection<PostPreview>> GetByAuthorId(int authorId, CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<PostPreviewCategoryDto>(DbConstants.BhsConnectionStringName, "blog.PostPreviewCategory_GetByAuthorId", new
        {
            authorId
        }, cancellationToken);
        return results.GroupBy(row => row.Slug)
            .Select(postGrouping => PostPreviewCategoryDto.ToDomainModel(postGrouping))
            .OrderByDescending(p => p.DatePublished)
            .ToList();
    }
}
