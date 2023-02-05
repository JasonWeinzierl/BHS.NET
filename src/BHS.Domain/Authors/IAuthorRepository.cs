using BHS.Contracts;

namespace BHS.Domain.Authors;

public interface IAuthorRepository
{
    Task<Author?> GetByUsername(string username, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default);
}
