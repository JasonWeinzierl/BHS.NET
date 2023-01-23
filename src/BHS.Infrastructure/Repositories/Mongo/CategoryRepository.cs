using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoClient _mongoClient;

    public CategoryRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<IReadOnlyCollection<CategorySummary>> GetAll(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .Unwind<PostDto, PostUnwoundCategoryDto>(x => x.Categories)
            .Group(x => x.Categories.Slug, x => new CategorySummary(
                x.Key,
                x.First().Categories.Name,
                x.Count()) // TODO: this count doesn't consider unpublished or deleted posts.
            )
            .ToListAsync(cancellationToken);

    public async Task<Category?> GetBySlug(string slug, CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<PostDto>("posts")
            .Aggregate()
            .Match(x => x.Categories.Any(y => y.Slug == slug))
            .Project(x => new Category(
                x.Categories.First().Slug,
                x.Categories.First().Name)
            )
            .FirstOrDefaultAsync(cancellationToken);
}
