using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.Domain.Repositories;

namespace BHS.DataAccess.Repositories
{
    public class ContactAlertRepository :  IContactAlertRepository
    {
        protected IDbExecuter E { get; }

        public ContactAlertRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public async Task<ContactAlert> Insert(ContactAlertRequest contactRequest)
        {
            var result = await E.ExecuteSprocQuerySingleOrDefault<ContactAlert>(DbConstants.BhsConnectionStringName, "dbo.ContactAlert_Insert", new
            {
                name = contactRequest.Name,
                emailAddress = contactRequest.EmailAddress,
                message = contactRequest.Message,
                dateRequested = contactRequest.DateRequested
            });
            return result ?? throw new InvalidOperationException("Output of contact alert insert was null (this should never happen).");
        }
    }
}
