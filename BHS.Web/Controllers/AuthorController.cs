using BHS.Contracts;
using BHS.Contracts.Blog;
using BHS.Model.Services;
using BHS.Model.Services.Blog;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Web.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IAuthorService _authorService;

        public AuthorController(
            IBlogService blogService,
            IAuthorService authorService)
        {
            _blogService = blogService;
            _authorService = authorService;
        }

        /// <summary>
        /// Get all authors.
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return Ok(await _authorService.GetAuthors());
        }

        /// <summary>
        /// Get an author.
        /// </summary>
        [HttpGet("{username}")]
        public async Task<ActionResult<Author>> GetAuthor(string username)
        {
            var author = await _authorService.GetAuthor(username);
            if (author == default) return NotFound();
            return Ok(author);
        }

        /// <summary>
        /// Get the posts of an author.
        /// </summary>
        [HttpGet("{username}/posts")]
        public async Task<ActionResult<IEnumerable<PostPreview>>> GetAuthorPosts(string username)
        {
            var posts = await _blogService.GetPostsByAuthor(username);
            if (posts == default) return NotFound();
            return Ok(posts);
        }
    }
}
