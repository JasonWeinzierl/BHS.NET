using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IContactAlertRepository
    {
        Task<ContactAlert> Insert(ContactAlertRequest contactRequest);
    }
}
