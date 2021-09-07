using BHS.Contracts;

namespace BHS.Domain.Repositories
{
    public interface IContactAlertRepository
    {
        Task<ContactAlert> Insert(ContactAlertRequest contactRequest);
    }
}
