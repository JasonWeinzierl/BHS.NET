using BHS.Contracts;
using BHS.DataAccess;
using BHS.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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

        public Task<IEnumerable<Author>> GetAuthors(bool doIncludeHidden = false)
        {
            return _authorRepository.GetAll(doIncludeHidden);
        }
    }
}
