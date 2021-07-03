using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.Domain.DataAccess
{
    public interface IContactAlertRepository
    {
        Task<ContactAlert> Insert(ContactAlertRequest contactRequest);
    }
}
