using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetByAuthorId(int authorId);
        Task<IEnumerable<Post>> GetByCategoryId(int categoryId);
        Task<Post> GetById(int id);
        Task<IEnumerable<Post>> Search(string text, DateTimeOffset? from, DateTimeOffset? to);
    }
}
