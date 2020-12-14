using BHS.Contracts.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetBySlug(string slug);
        Task<IEnumerable<Category>> GetByPostSlug(string postSlug);
    }
}
