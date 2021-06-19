using BHS.Contracts.Leadership;
using BHS.Model.DataAccess;
using BHS.Model.Services.Leadership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Leadership
{
    public class LeadershipService : ILeadershipService
    {
        private readonly ILeadershipRepository _leadershipRepository;

        public LeadershipService(ILeadershipRepository leadershipRepository)
        {
            _leadershipRepository = leadershipRepository;
        }

        public Task<IList<Officer>> GetOfficers()
        {
            return _leadershipRepository.GetCurrentOfficers();
        }

        public Task<IList<Director>> GetDirectors()
        {
            return _leadershipRepository.GetCurrentDirectors();
        }
    }
}
