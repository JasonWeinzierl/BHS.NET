using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Model.DataAccess;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class ContactAlertRepository :  IContactAlertRepository
    {
        protected IDbExecuter E { get; }

        public ContactAlertRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public Task<ContactAlert> Insert(ContactAlertRequest contactRequest)
        {
            return E.QuerySingleOrDefaultAsync<ContactAlert>(Constants.bhsConnectionStringName, "dbo.ContactAlert_Insert", new
            {
                name = contactRequest.Name,
                emailAddress = contactRequest.EmailAddress,
                message = contactRequest.Message,
                dateRequested = contactRequest.DateRequested
            });
        }
    }
}
