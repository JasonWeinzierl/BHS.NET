using BHS.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Domain.Services
{
    public interface IAuthorService
    {
        Task<Author?> GetAuthor(string userName);
        Task<IReadOnlyCollection<Author>> GetAuthors();
    }
}
