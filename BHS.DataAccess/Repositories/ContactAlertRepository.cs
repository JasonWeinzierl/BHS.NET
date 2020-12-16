using BHS.Contracts;
using BHS.DataAccess.Core;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class ContactAlertRepository : SprocRepositoryBase
        , IContactAlertRepository
    {
        public ContactAlertRepository(IDbConnectionFactory factory) : base(factory) { }

        public async Task<ContactAlert> Insert(ContactAlertRequest contactRequest)
        {
            return await ExecuteReaderAsync(Constants.bhsConnectionStringName, "dbo.ContactAlert_Insert", cmd =>
            {
                cmd.Parameters.Add(CreateParameter(cmd, "@name", contactRequest.Name));
                cmd.Parameters.Add(CreateParameter(cmd, "@emailAddress", contactRequest.EmailAddress));
                cmd.Parameters.Add(CreateParameter(cmd, "@message", contactRequest.Message));
                cmd.Parameters.Add(CreateParameter(cmd, "@dateRequested", contactRequest.DateRequested, DbType.DateTimeOffset));
            }, GetContactAlert).SingleOrDefaultAsync();
        }

        private static ContactAlert GetContactAlert(IDataRecord dr)
        {
            return new ContactAlert(
                ToInt(dr["Id"]),
                ToString(dr["Name"]),
                ToString(dr["EmailAddress"]),
                ToString(dr["Message"]),
                ToNullableDateTimeOffset(dr["DateRequested"]),
                ToDateTimeOffset(dr["DateCreated"])
                );
        }
    }
}
