using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController : ControllerBase
{
    private readonly IPostPreviewRepository _postRepo;

    public AuthorController(
        IPostPreviewRepository postRepo)
    {
        _postRepo = postRepo;
    }

    /// <summary>
    /// Get the posts of an author.
    /// </summary>
    [HttpGet("{username}/posts")]
    public async Task<ActionResult<IEnumerable<PostPreview>>> GetAuthorPosts(string username, CancellationToken cancellationToken = default)
    {
        var posts = await _postRepo.GetByAuthorUsername(username, cancellationToken);
        if (posts is null) return NotFound();
        else return Ok(posts);
    }
}
