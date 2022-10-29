using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership;

public class LeadershipService : ILeadershipService
{
    private readonly ILeadershipRepository _leadershipRepository;

    public LeadershipService(ILeadershipRepository leadershipRepository)
    {
        _leadershipRepository = leadershipRepository;
    }

    public Task<IReadOnlyCollection<Officer>> GetOfficers(CancellationToken cancellationToken = default)
        => _leadershipRepository.GetCurrentOfficers(cancellationToken);

    public Task<IReadOnlyCollection<Director>> GetDirectors(CancellationToken cancellationToken = default)
        => _leadershipRepository.GetCurrentDirectors(cancellationToken);
}
