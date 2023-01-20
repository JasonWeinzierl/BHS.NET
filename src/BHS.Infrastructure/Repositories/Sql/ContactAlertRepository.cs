using BHS.Contracts;
using BHS.Domain.ContactUs;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Repositories.Sql.Models;

namespace BHS.Infrastructure.Repositories.Sql;

public class ContactAlertRepository : IContactAlertRepository
{
    protected IDbExecuter E { get; }

    public ContactAlertRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<ContactAlert> Insert(ContactAlertRequest contactRequest, CancellationToken cancellationToken = default)
    {
        var result = await E.ExecuteSprocQuerySingleOrDefault<ContactAlertDTO>(DbConstants.BhsConnectionStringName, "dbo.ContactAlert_Insert", new
        {
            name = contactRequest.Name,
            emailAddress = contactRequest.EmailAddress,
            message = contactRequest.Message,
            dateRequested = contactRequest.DateRequested
        }, cancellationToken);
        return result?.ToDomainModel() ?? throw new InvalidOperationException("Output of contact alert insert was null (this should never happen).");
    }
}
