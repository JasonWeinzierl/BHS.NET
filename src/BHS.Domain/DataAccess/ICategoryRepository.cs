using BHS.Contracts.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Domain.DataAccess
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyCollection<CategorySummary>> GetAll();
        Task<Category?> GetBySlug(string slug);
    }
}
