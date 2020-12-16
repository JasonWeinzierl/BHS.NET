using BHS.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor(string userName);
        IAsyncEnumerable<Author> GetAuthors(bool doIncludeHidden = false);
    }
}
