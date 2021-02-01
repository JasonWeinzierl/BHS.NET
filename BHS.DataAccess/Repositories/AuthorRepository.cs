using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        protected IQuerier Q { get; }

        public AuthorRepository(IQuerier querier)
        {
            Q = querier;
        }

        public Task<IEnumerable<Author>> GetAll()
        {
            return Q.QueryAsync<Author>(Constants.bhsConnectionStringName, "dbo.Author_GetAll");
        }

        public Task<Author> GetByUserName(string userName)
        {
            return Q.QuerySingleOrDefaultAsync<Author>(Constants.bhsConnectionStringName, "dbo.Author_GetByUserName", new { userName });
        }
    }
}
