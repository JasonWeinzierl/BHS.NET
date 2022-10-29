using BHS.Contracts;

namespace BHS.Domain.ContactUs;

public interface IContactUsService
{
    Task<ContactAlert?> AddRequest(ContactAlertRequest request, CancellationToken cancellationToken = default);
}
