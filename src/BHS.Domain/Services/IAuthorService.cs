using BHS.Contracts;

namespace BHS.Domain.Services
{
    public interface IAuthorService
    {
        Task<Author?> GetAuthor(string userName);
        Task<IReadOnlyCollection<Author>> GetAuthors();
    }
}
