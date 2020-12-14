using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Blog
{
    public interface IBlogService
    {
        Task<Post> GetPost(string slug);
        Task<IEnumerable<Post>> SearchPosts(string text, DateTime? from, DateTime? to);
        Task<IEnumerable<Category>> GetCategoriesByPost(string slug);
        Task<Category> GetCategory(string slug);
        Task<IEnumerable<Post>> GetPostsByCategory(string slug);
        Task<IEnumerable<Post>> GetPostsByAuthor(string username);
    }
}
