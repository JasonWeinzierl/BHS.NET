using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Collections.Generic;
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

        public Task<IEnumerable<Author>> GetAll()
        {
            return E.QueryAsync<Author>(Constants.bhsConnectionStringName, "dbo.Author_GetAll");
        }

        public Task<Author> GetByUserName(string userName)
        {
            return E.QuerySingleOrDefaultAsync<Author>(Constants.bhsConnectionStringName, "dbo.Author_GetByUserName", new { userName });
        }
    }
}
