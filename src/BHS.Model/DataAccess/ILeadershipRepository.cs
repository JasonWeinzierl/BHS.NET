using BHS.Contracts.Leadership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface ILeadershipRepository
    {
        Task<IReadOnlyCollection<Officer>> GetCurrentOfficers();
        Task<IReadOnlyCollection<Director>> GetCurrentDirectors();
    }
}
