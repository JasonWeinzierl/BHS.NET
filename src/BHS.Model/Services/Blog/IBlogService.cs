using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.Services.Blog
{
    public interface IBlogService
    {
        Task<Post?> GetPost(string slug);
        Task<IReadOnlyCollection<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to);
        Task<IReadOnlyCollection<CategorySummary>> GetCategories();
        Task<CategoryPosts?> GetCategory(string slug);
        Task<IReadOnlyCollection<PostPreview>> GetPostsByAuthor(string username);
    }
}
