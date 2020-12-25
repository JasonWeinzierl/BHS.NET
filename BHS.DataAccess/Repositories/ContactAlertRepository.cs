using BHS.Contracts;
using BHS.DataAccess.Core;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class ContactAlertRepository :  IContactAlertRepository
    {
        protected IQuerier Q { get; }

        public ContactAlertRepository(IQuerier querier)
        {
            Q = querier;
        }

        public async Task<ContactAlert> Insert(ContactAlertRequest contactRequest)
        {
            return await Q.ExecuteReaderAsync(Constants.bhsConnectionStringName, "dbo.ContactAlert_Insert", cmd =>
            {
                cmd.Parameters.Add(cmd.CreateParameter("@name", contactRequest.Name));
                cmd.Parameters.Add(cmd.CreateParameter("@emailAddress", contactRequest.EmailAddress));
                cmd.Parameters.Add(cmd.CreateParameter("@message", contactRequest.Message));
                cmd.Parameters.Add(cmd.CreateParameter("@dateRequested", contactRequest.DateRequested, DbType.DateTimeOffset));
            }, GetContactAlert).SingleOrDefaultAsync();
        }

        private static ContactAlert GetContactAlert(IDataRecord dr)
        {
            return new ContactAlert(
                dr.CastInt("Id"),
                dr.CastString("Name"),
                dr.CastString("EmailAddress"),
                dr.CastString("Message"),
                dr.CastNullableDateTimeOffset("DateRequested"),
                dr.CastDateTimeOffset("DateCreated")
                );
        }
    }
}
