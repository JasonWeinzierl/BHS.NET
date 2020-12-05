using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Blog
{
    public interface IBlogService
    {
        Task<Post> GetPost(int id);
        Task<IEnumerable<Post>> SearchPosts(string text, DateTime? from, DateTime? to);
        Task<IEnumerable<Category>> GetCategoriesByPost(int id);
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Post>> GetPostsByCategory(int id);
        Task<IEnumerable<Post>> GetPostsByAuthor(string username);
    }
}
