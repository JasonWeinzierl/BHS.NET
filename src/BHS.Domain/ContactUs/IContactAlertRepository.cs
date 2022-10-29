using BHS.Contracts;

namespace BHS.Domain.ContactUs;

public interface IContactAlertRepository
{
    Task<ContactAlert> Insert(ContactAlertRequest contactRequest, CancellationToken cancellationToken = default);
}
