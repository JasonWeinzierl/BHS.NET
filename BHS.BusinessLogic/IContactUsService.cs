using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.BusinessLogic
{
    public interface IContactUsService
    {
        Task AddRequest(ContactAlertRequest request);
    }
}
