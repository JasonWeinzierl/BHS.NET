using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Repositories.Sql.Models;

namespace BHS.Infrastructure.Repositories.Sql;

public class PostRepository : IPostRepository
{
    protected IDbExecuter E { get; }

    public PostRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<Post?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var (posts, categories) = await E.ExecuteSprocQueryMultiple<PostDto, CategoryDto>(DbConstants.BhsConnectionStringName, "blog.Post_GetBySlug", new { slug }, cancellationToken);
        return posts.SingleOrDefault()?.ToDomainModel(categories.Select(c => c.ToDomainModel()));
    }
}
