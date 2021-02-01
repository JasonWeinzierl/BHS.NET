using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
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

        public Task<ContactAlert> Insert(ContactAlertRequest contactRequest)
        {
            return Q.QuerySingleOrDefaultAsync<ContactAlert>(Constants.bhsConnectionStringName, "dbo.ContactAlert_Insert", new
            {
                name = contactRequest.Name,
                emailAddress = contactRequest.EmailAddress,
                message = contactRequest.Message,
                dateRequested = contactRequest.DateRequested
            });
        }
    }
}
