using BHS.Contracts.Blog;
using BHS.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers
{
    [ApiController]
    [Route("api/blog")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// Get all posts, with optional search criteria
        /// </summary>
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostPreview>>> SearchPosts(string? q, DateTime? from, DateTime? to)
            => Ok(await _blogService.SearchPosts(q, from, to));

        /// <summary>
        /// Get a post
        /// </summary>
        [HttpGet("posts/{slug}")]
        public async Task<ActionResult<Post>> GetPost(string slug)
        {
            var post = await _blogService.GetPost(slug);
            if (post is null)
                return NotFound();
            return Ok(post);
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategorySummary>>> GetCategories()
            => Ok(await _blogService.GetCategories());

        /// <summary>
        /// Get a category and its posts.
        /// </summary>
        [HttpGet("categories/{slug}")]
        public async Task<ActionResult<CategoryPosts>> GetCategory(string slug)
        {
            var category = await _blogService.GetCategory(slug);
            if (category is null)
                return NotFound();
            return Ok(category);
        }
    }
}
