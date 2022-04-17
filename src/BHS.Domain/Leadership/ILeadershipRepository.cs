using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership
{
    public interface ILeadershipRepository
    {
        Task<IReadOnlyCollection<Officer>> GetCurrentOfficers();
        Task<IReadOnlyCollection<Director>> GetCurrentDirectors();
    }
}
