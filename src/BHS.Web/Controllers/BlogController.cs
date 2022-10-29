using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

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
    public async Task<ActionResult<IEnumerable<PostPreview>>> SearchPosts(string? q, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
        => Ok(await _blogService.SearchPosts(q, from, to, cancellationToken));

    /// <summary>
    /// Get a post
    /// </summary>
    [HttpGet("posts/{slug}")]
    public async Task<ActionResult<Post>> GetPost(string slug, CancellationToken cancellationToken = default)
    {
        var post = await _blogService.GetPost(slug, cancellationToken);
        if (post is null) return NotFound();
        else return Ok(post);
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategorySummary>>> GetCategories(CancellationToken cancellationToken = default)
        => Ok(await _blogService.GetCategories(cancellationToken));

    /// <summary>
    /// Get a category and its posts.
    /// </summary>
    [HttpGet("categories/{slug}")]
    public async Task<ActionResult<CategoryPosts>> GetCategory(string slug, CancellationToken cancellationToken = default)
    {
        var category = await _blogService.GetCategory(slug, cancellationToken);
        if (category is null) return NotFound();
        else return Ok(category);
    }
}
