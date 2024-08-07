﻿using BHS.Contracts.Blog;
using BHS.Domain.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly IPostPreviewRepository _previewRepo;
    private readonly IPostRepository _postRepo;
    private readonly ICategoryRepository _categoryRepo;

    public BlogController(
        IPostPreviewRepository previewRepo,
        IPostRepository postRepo,
        ICategoryRepository categoryRepo)
    {
        _previewRepo = previewRepo;
        _postRepo = postRepo;
        _categoryRepo = categoryRepo;
    }

    /// <summary>
    /// Get all posts, with optional search criteria.
    /// </summary>
    [HttpGet("posts")]
    public async Task<ActionResult<IEnumerable<PostPreview>>> SearchPosts(string? q, DateTime? from, DateTime? to, CancellationToken cancellationToken)
        => Ok(await _previewRepo.Search(q, from, to, cancellationToken));

    /// <summary>
    /// Create a new post.
    /// </summary>
    [HttpPost("posts")]
    [Authorize(AuthConfig.BlogWriteAccess)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Post>> CreatePost(PostRequest request, CancellationToken cancellationToken)
    {
        string slug = await _postRepo.Insert(request, cancellationToken);
        var post = await _postRepo.GetBySlug(slug, cancellationToken);
        return CreatedAtAction(nameof(GetPost), new { slug }, post);
    }

    /// <summary>
    /// Get a post.
    /// </summary>
    [HttpGet("posts/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Post>> GetPost(string slug, CancellationToken cancellationToken)
    {
        var post = await _postRepo.GetBySlug(slug, cancellationToken);
        if (post is null) return NotFound();
        else return Ok(post);
    }

    /// <summary>
    /// Update a post.
    /// </summary>
    [HttpPut("posts/{slug}")]
    [Authorize(AuthConfig.BlogWriteAccess)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Post>> UpdatePost(string slug, PostRequest request, CancellationToken cancellationToken)
    {
        var post = await _postRepo.Update(slug, request, cancellationToken);
        if (post is null) return NotFound();
        else return Ok(post);
    }

    /// <summary>
    /// Delete a post.
    /// </summary>
    [HttpDelete("posts/{slug}")]
    [Authorize(AuthConfig.BlogWriteAccess)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletePost(string slug, CancellationToken cancellationToken)
    {
        bool exists = await _postRepo.Delete(slug, cancellationToken);
        if (!exists) return NotFound();
        else return NoContent();
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<CategorySummary>>> GetCategories(CancellationToken cancellationToken)
        => Ok(await _categoryRepo.GetAll(cancellationToken));

    /// <summary>
    /// Get a category and its posts.
    /// </summary>
    [HttpGet("categories/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryPosts>> GetCategory(string slug, CancellationToken cancellationToken)
    {
        var category = await _previewRepo.GetCategoryPosts(slug, cancellationToken);
        if (category is null) return NotFound();
        else return Ok(category);
    }
}
