using BHS.Contracts;

namespace BHS.Domain.Services
{
    public interface IContactUsService
    {
        Task<ContactAlert?> AddRequest(ContactAlertRequest request);
    }
}
