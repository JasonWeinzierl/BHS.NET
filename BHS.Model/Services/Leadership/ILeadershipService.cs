using BHS.Contracts.Leadership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.Services.Leadership
{
    public interface ILeadershipService
    {
        Task<IList<Officer>> GetOfficers();
        Task<IList<Director>> GetDirectors();
    }
}
