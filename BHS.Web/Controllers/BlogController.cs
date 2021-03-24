using BHS.Contracts.Blog;
using BHS.Model.Services.Blog;
//using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        {
            return Ok(await _blogService.SearchPosts(q, from, to));
        }

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

        ///// <summary>
        ///// Set the categories of a post
        ///// </summary>
        //[HttpPut("posts/{id}/categories")]
        //public IActionResult SetPostCategoires(int id, [FromBody]IEnumerable<int> categoryIds)
        //{
        //    return Ok(categoryIds);
        //}

        ///// <summary>
        ///// Add a post
        ///// </summary>
        //[HttpPost("posts")]
        //public IActionResult AddPost([FromBody] Post post)
        //{
        //    return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        //}

        ///// <summary>
        ///// Edit a post
        ///// </summary>
        //[HttpPatch("posts/{id}")]
        //public IActionResult EditPost(int id, [FromBody] JsonPatchDocument<Post> patchDocument)
        //{
        //    var post = new Post { Id = id };

        //    patchDocument.ApplyTo(post);

        //    return Ok(post);
        //}

        ///// <summary>
        ///// Hide a post
        ///// </summary>
        //[HttpDelete("posts/{id}")]
        //public IActionResult DisablePost(int id)
        //{
        //    return NoContent();
        //}


        /// <summary>
        /// Get all categories.
        /// </summary>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategorySummary>>> GetCategories()
        {
            return Ok(await _blogService.GetCategories());
        }

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

        ///// <summary>
        ///// Hide a category
        ///// </summary>
        //[HttpDelete("categories/{id}")]
        //public IActionResult DisableCategory(int id)
        //{
        //    return NoContent();
        //}
    }
}
