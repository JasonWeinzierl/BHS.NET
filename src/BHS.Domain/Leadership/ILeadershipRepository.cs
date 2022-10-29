using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership;

public interface ILeadershipRepository
{
    Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default);
}
