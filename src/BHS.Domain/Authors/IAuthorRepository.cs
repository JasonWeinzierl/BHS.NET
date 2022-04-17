using BHS.Contracts;

namespace BHS.Domain.Authors
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByUserName(string userName);
        Task<IReadOnlyCollection<Author>> GetAll();
    }
}
