using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetByAuthorId(int authorId);
        Task<IEnumerable<Post>> GetByCategorySlug(string categorySlug);
        Task<Post> GetBySlug(string slug);
        Task<IEnumerable<Post>> Search(string text, DateTimeOffset? from, DateTimeOffset? to);
    }
}
