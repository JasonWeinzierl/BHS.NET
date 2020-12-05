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

        public async Task<IEnumerable<Author>> GetAll(bool doIncludeHidden = false)
        {
            return await ExecuteReaderAsync<List<Author>>(Constants.bhsConnectionStringName, "dbo.Author_GetAll", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@doIncludeHidden", doIncludeHidden, DbType.Boolean));
            }, FillAuthors);
        }

        public async Task<Author> GetByUserName(string userName)
        {
            return (await ExecuteReaderAsync<List<Author>>(Constants.bhsConnectionStringName, "dbo.Author_GetByUserName", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@userName", userName));
            }, FillAuthors)).FirstOrDefault();
        }

        private void FillAuthors(IDataReader dr, ref List<Author> models)
        {
            var model = new Author(
                ToInt(dr["Id"]),
                ToString(dr["DisplayName"]),
                ToString(dr["Name"]),
                ToBool(dr["IsVisible"])
                );
            models.Add(model);
        }
    }
}
