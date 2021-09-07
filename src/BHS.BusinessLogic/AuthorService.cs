using BHS.Contracts;
using BHS.Domain.Repositories;
using BHS.Domain.Services;
using Microsoft.Extensions.Logging;

namespace BHS.BusinessLogic
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
        {
            return _authorRepository.GetByUserName(username);
        }

        public Task<IReadOnlyCollection<Author>> GetAuthors()
        {
            return _authorRepository.GetAll();
        }
    }
}
