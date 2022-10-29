using BHS.Contracts;
using Microsoft.Extensions.Logging;

namespace BHS.Domain.Authors;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ILogger _logger;

    public AuthorService(
        IAuthorRepository authorRepository,
        ILogger<AuthorService> logger)
    {
        _authorRepository = authorRepository;
        _logger = logger;
    }

    public Task<Author?> GetAuthor(string username, CancellationToken cancellationToken = default)
        => _authorRepository.GetByUserName(username, cancellationToken);

    public Task<IReadOnlyCollection<Author>> GetAuthors(CancellationToken cancellationToken = default)
        => _authorRepository.GetAll(cancellationToken);
}
