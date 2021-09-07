using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Domain.Repositories;

namespace BHS.DataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        protected IDbExecuter E { get; }

        public AuthorRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public async Task<IReadOnlyCollection<Author>> GetAll()
        {
            var authors = await E.ExecuteSprocQuery<Author>(DbConstants.BhsConnectionStringName, "dbo.Author_GetAll");
            return authors.ToList();
        }

        public Task<Author?> GetByUserName(string userName)
        {
            return E.ExecuteSprocQuerySingleOrDefault<Author>(DbConstants.BhsConnectionStringName, "dbo.Author_GetByUserName", new { userName });
        }
    }
}
