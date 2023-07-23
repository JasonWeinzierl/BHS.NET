using BHS.Contracts;

namespace BHS.Domain.Authors;

public interface IAuthorRepository
{
    Task<IReadOnlyCollection<Author>> GetByAuthUserId(string authUserId, CancellationToken cancellationToken = default);
}
