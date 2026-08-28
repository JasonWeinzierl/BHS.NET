using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership;

public interface ILeadershipRepository
{
    Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default);
    Task CreateOffice(OfficeRequest office, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Officer>> UpdateOfficers(IReadOnlyCollection<OfficerRequest> officers, CancellationToken cancellationToken = default);
    Task<bool> DeleteOffice(string title, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Director>> AddDirectors(IReadOnlyCollection<DirectorRequest> directors, CancellationToken cancellationToken = default);
    Task<bool> DeleteDirector(string name, int year, CancellationToken cancellationToken = default);
}
