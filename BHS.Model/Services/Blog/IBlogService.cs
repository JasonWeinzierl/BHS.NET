using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.Services.Blog
{
    public interface IBlogService
    {
        Task<Post?> GetPost(string slug);
        Task<IEnumerable<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to);
        Task<IEnumerable<Category>> GetCategoriesByPost(string slug);
        Task<Category?> GetCategory(string slug);
        Task<IEnumerable<PostPreview>> GetPostsByCategory(string slug);
        Task<IEnumerable<PostPreview>> GetPostsByAuthor(string username);
    }
}
