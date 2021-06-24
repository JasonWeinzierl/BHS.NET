using BHS.Contracts.Leadership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.Services.Leadership
{
    public interface ILeadershipService
    {
        Task<IReadOnlyCollection<Officer>> GetOfficers();
        Task<IReadOnlyCollection<Director>> GetDirectors();
    }
}
