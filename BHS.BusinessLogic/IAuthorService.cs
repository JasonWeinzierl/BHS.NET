using BHS.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor(string userName);
        Task<IEnumerable<Author>> GetAuthors();
    }
}
