using BHS.Contracts;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public IAsyncEnumerable<Author> GetAll(bool doIncludeHidden = false)
        {
            return Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "dbo.Author_GetAll", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@doIncludeHidden", doIncludeHidden, DbType.Boolean));
            }, GetAuthor);
        }

        public async Task<Author> GetByUserName(string userName)
        {
            return await Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "dbo.Author_GetByUserName", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@userName", userName));
            }, GetAuthor).SingleOrDefaultAsync();
        }

        private static Author GetAuthor(IDataRecord dr)
        {
            return new Author(
                dr.CastInt("Id"),
                dr.CastString("DisplayName"),
                dr.CastString("Name"),
                dr.CastBool("IsVisible")
                );
        }
    }
}
