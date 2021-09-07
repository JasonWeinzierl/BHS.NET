using BHS.Contracts.Leadership;

namespace BHS.Domain.Services
{
    public interface ILeadershipService
    {
        Task<IReadOnlyCollection<Officer>> GetOfficers();
        Task<IReadOnlyCollection<Director>> GetDirectors();
    }
}
