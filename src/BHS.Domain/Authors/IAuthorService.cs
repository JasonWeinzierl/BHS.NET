using BHS.Contracts;

namespace BHS.Domain.Authors
{
    public interface IAuthorService
    {
        Task<Author?> GetAuthor(string userName);
        Task<IReadOnlyCollection<Author>> GetAuthors();
    }
}
