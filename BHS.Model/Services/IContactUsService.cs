using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.Model.Services
{
    public interface IContactUsService
    {
        Task AddRequest(ContactAlertRequest request);
    }
}
