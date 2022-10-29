using BHS.Contracts;

namespace BHS.Domain.Authors;

public interface IAuthorService
{
    Task<Author?> GetAuthor(string userName, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Author>> GetAuthors(CancellationToken cancellationToken = default);
}
