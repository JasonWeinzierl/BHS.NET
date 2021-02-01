using BHS.Contracts;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IContactAlertRepository
    {
        Task<ContactAlert> Insert(ContactAlertRequest contactRequest);
    }
}
