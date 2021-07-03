using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Domain.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var authors = await E.QueryAsync<Author>(Constants.bhsConnectionStringName, "dbo.Author_GetAll");
            return authors.ToList();
        }

        public Task<Author?> GetByUserName(string userName)
        {
            return E.QuerySingleOrDefaultAsync<Author>(Constants.bhsConnectionStringName, "dbo.Author_GetByUserName", new { userName });
        }
    }
}
