using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership;

public interface ILeadershipService
{
    Task<IReadOnlyCollection<Officer>> GetOfficers(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Director>> GetDirectors(CancellationToken cancellationToken = default);
}
