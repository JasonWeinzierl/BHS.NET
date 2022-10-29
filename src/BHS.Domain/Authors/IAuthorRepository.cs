using BHS.Contracts;

namespace BHS.Domain.Authors;

public interface IAuthorRepository
{
    Task<Author?> GetByUserName(string userName, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default);
}
