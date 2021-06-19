using BHS.Contracts.Leadership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface ILeadershipRepository
    {
        Task<IList<Officer>> GetCurrentOfficers();
        Task<IList<Director>> GetCurrentDirectors();
    }
}
