using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.Model.Services
{
    public interface IContactUsService
    {
        Task<ContactAlert?> AddRequest(ContactAlertRequest request);
    }
}
