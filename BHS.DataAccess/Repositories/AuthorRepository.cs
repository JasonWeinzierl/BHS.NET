using BHS.Contracts;
using BHS.DataAccess.Core;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class AuthorRepository : SprocRepositoryBase
        , IAuthorRepository
    {
        public AuthorRepository(IDbConnectionFactory factory) : base(factory) { }

        public IAsyncEnumerable<Author> GetAll(bool doIncludeHidden = false)
        {
            return ExecuteReaderAsync(Constants.bhsConnectionStringName, "dbo.Author_GetAll", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@doIncludeHidden", doIncludeHidden, DbType.Boolean));
            }, GetAuthor);
        }

        public async Task<Author> GetByUserName(string userName)
        {
            return await ExecuteReaderAsync(Constants.bhsConnectionStringName, "dbo.Author_GetByUserName", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@userName", userName));
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
