using BHS.Contracts.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategorySummary>> GetAll();
        Task<CategoryPosts?> GetBySlug(string slug);
    }
}
