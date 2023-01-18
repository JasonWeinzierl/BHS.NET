using BHS.Contracts;
using BHS.Domain.Authors;
using BHS.Infrastructure.Core;

namespace BHS.Infrastructure.Repositories.Sql;

public class AuthorRepository : IAuthorRepository
{
    protected IDbExecuter E { get; }

    public AuthorRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default)
    {
        var authors = await E.ExecuteSprocQuery<Author>(DbConstants.BhsConnectionStringName, "dbo.Author_GetAll", cancellationToken: cancellationToken);
        return authors.ToList();
    }

    public Task<Author?> GetByUserName(string userName, CancellationToken cancellationToken = default)
    {
        return E.ExecuteSprocQuerySingleOrDefault<Author>(DbConstants.BhsConnectionStringName, "dbo.Author_GetByUserName", new { userName }, cancellationToken);
    }
}
