using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership
{
    public class LeadershipService : ILeadershipService
    {
        private readonly ILeadershipRepository _leadershipRepository;

        public LeadershipService(ILeadershipRepository leadershipRepository)
        {
            _leadershipRepository = leadershipRepository;
        }

        public Task<IReadOnlyCollection<Officer>> GetOfficers()
            => _leadershipRepository.GetCurrentOfficers();

        public Task<IReadOnlyCollection<Director>> GetDirectors()
            => _leadershipRepository.GetCurrentDirectors();
    }
}
