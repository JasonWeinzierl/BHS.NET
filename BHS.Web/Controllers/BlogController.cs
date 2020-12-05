using BHS.BusinessLogic.Blog;
using BHS.Contracts.Blog;
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
        public async Task<ActionResult<IEnumerable<Post>>> SearchPosts(string q, DateTime? from, DateTime? to)
        {
            return Ok(await _blogService.SearchPosts(q, from, to));
        }

        /// <summary>
        /// Get a post
        /// </summary>
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _blogService.GetPost(id);
            if (post is null) return NotFound();
            return Ok(post);
        }

        /// <summary>
        /// Get the categories of a post
        /// </summary>
        [HttpGet("posts/{id}/categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetPostCategories(int id)
        {
            return Ok(await _blogService.GetCategoriesByPost(id));
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
        /// Get a category
        /// </summary>
        [HttpGet("categories/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _blogService.GetCategory(id);
            if (category is null) return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Get the posts of a category
        /// </summary>
        [HttpGet("categories/{id}/posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetCategoryPosts(int id)
        {
            return Ok(await _blogService.GetPostsByCategory(id));
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
