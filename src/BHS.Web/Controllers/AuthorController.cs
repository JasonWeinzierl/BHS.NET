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
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(CancellationToken cancellationToken = default)
        => Ok(await _authorService.GetAuthors(cancellationToken));

    /// <summary>
    /// Get an author.
    /// </summary>
    [HttpGet("{username}")]
    public async Task<ActionResult<Author>> GetAuthor(string username, CancellationToken cancellationToken = default)
    {
        var author = await _authorService.GetAuthor(username, cancellationToken);
        if (author is null) return NotFound();
        else return Ok(author);
    }

    /// <summary>
    /// Get the posts of an author.
    /// </summary>
    [HttpGet("{username}/posts")]
    public async Task<ActionResult<IEnumerable<PostPreview>>> GetAuthorPosts(string username, CancellationToken cancellationToken = default)
    {
        var posts = await _blogService.GetPostsByAuthor(username, cancellationToken);
        if (posts is null) return NotFound();
        else return Ok(posts);
    }
}
