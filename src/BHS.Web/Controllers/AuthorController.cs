using BHS.Contracts;
using BHS.Contracts.Blog;
using BHS.Domain.Authors;
using BHS.Domain.Blog;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IPostPreviewRepository _postRepo;
    private readonly IAuthorRepository _authorRepo;

    public AuthorController(
        IPostPreviewRepository postRepo,
        IAuthorRepository authorRepo)
    {
        _postRepo = postRepo;
        _authorRepo = authorRepo;
    }

    /// <summary>
    /// Search authors for a given user.
    /// </summary>
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(string authUserId, CancellationToken cancellationToken = default)
        => Ok(await _authorRepo.GetByAuthUserId(authUserId, cancellationToken));

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
