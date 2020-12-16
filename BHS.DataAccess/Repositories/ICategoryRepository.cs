using BHS.Contracts.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetBySlug(string slug);
        IAsyncEnumerable<Category> GetByPostSlug(string postSlug);
    }
}
