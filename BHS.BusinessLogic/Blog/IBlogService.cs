using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Blog
{
    public interface IBlogService
    {
        Task<Post> GetPost(string slug);
        IAsyncEnumerable<PostPreview> SearchPosts(string text, DateTime? from, DateTime? to);
        IAsyncEnumerable<Category> GetCategoriesByPost(string slug);
        Task<Category> GetCategory(string slug);
        IAsyncEnumerable<PostPreview> GetPostsByCategory(string slug);
        IAsyncEnumerable<PostPreview> GetPostsByAuthor(string username);
    }
}
