using BHS.Contracts;
using BHS.Model.DataAccess;
using BHS.Model.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task<Author> GetAuthor(string username)
        {
            return _authorRepository.GetByUserName(username);
        }

        public Task<IEnumerable<Author>> GetAuthors()
        {
            return _authorRepository.GetAll();
        }
    }
}
