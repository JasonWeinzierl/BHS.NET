using BHS.Contracts.Leadership;

namespace BHS.Domain.Leadership
{
    public interface ILeadershipService
    {
        Task<IReadOnlyCollection<Officer>> GetOfficers();
        Task<IReadOnlyCollection<Director>> GetDirectors();
    }
}
