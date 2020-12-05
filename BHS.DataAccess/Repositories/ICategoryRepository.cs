using BHS.Contracts.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetById(int id);
        Task<IEnumerable<Category>> GetByPostId(int postId);
    }
}
