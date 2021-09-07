using BHS.Contracts;

namespace BHS.Domain.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByUserName(string userName);
        Task<IReadOnlyCollection<Author>> GetAll();
    }
}
