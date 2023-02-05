using BHS.Contracts;
using BHS.Contracts.Blog;
using BHS.Domain.Authors;
using BHS.Domain.Blog;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/author")]
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
    /// Get all authors.
    /// </summary>
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(CancellationToken cancellationToken = default)
        => Ok(await _authorRepo.GetAll(cancellationToken));

    /// <summary>
    /// Get an author.
    /// </summary>
    [HttpGet("{username}")]
    public async Task<ActionResult<Author>> GetAuthor(string username, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepo.GetByUsername(username, cancellationToken);
        if (author is null) return NotFound();
        else return Ok(author);
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
