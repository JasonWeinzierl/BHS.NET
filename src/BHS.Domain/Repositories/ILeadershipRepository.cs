using BHS.Contracts.Leadership;

namespace BHS.Domain.Repositories
{
    public interface ILeadershipRepository
    {
        Task<IReadOnlyCollection<Officer>> GetCurrentOfficers();
        Task<IReadOnlyCollection<Director>> GetCurrentDirectors();
    }
}
