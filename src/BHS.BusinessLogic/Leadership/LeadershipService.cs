using BHS.Contracts.Leadership;
using BHS.Domain.DataAccess;
using BHS.Domain.Services.Leadership;
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

        public Task<IReadOnlyCollection<Officer>> GetOfficers()
        {
            return _leadershipRepository.GetCurrentOfficers();
        }

        public Task<IReadOnlyCollection<Director>> GetDirectors()
        {
            return _leadershipRepository.GetCurrentDirectors();
        }
    }
}
