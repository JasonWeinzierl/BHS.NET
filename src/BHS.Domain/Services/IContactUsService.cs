using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.Domain.Services
{
    public interface IContactUsService
    {
        Task<ContactAlert?> AddRequest(ContactAlertRequest request);
    }
}
