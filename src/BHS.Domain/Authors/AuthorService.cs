using BHS.Contracts;
using Microsoft.Extensions.Logging;

namespace BHS.Domain.Authors
{
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

        public Task<Author?> GetAuthor(string username)
            => _authorRepository.GetByUserName(username);

        public Task<IReadOnlyCollection<Author>> GetAuthors()
            => _authorRepository.GetAll();
    }
}
