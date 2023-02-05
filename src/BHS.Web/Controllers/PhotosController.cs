using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
    private readonly IAlbumRepository _albumRepo;

    public PhotosController(IAlbumRepository albumRepo)
    {
        _albumRepo = albumRepo;
    }

    /// <summary>
    /// Gets all albums.
    /// </summary>
    [HttpGet("albums")]
    public async Task<ActionResult<IEnumerable<Album>>> GetAlbums(CancellationToken cancellationToken = default)
        => Ok(await _albumRepo.GetAll(cancellationToken));

    /// <summary>
    /// Gets an album and its photos.
    /// </summary>
    [HttpGet("albums/{albumSlug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbumPhotos))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlbumPhotos>> GetAlbum(string albumSlug, CancellationToken cancellationToken = default)
    {
        var album = await _albumRepo.GetBySlug(albumSlug, cancellationToken);
        if (album is null) return NotFound();
        else return Ok(album);
    }

    /// <summary>
    /// Gets a photo for a particular album.
    /// </summary>
    [HttpGet("albums/{albumSlug}/photos/{photoId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Photo))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Photo>> GetPhoto(string albumSlug, string photoId, CancellationToken cancellationToken)
    {
        var photo = await _albumRepo.GetPhoto(albumSlug, photoId, cancellationToken);
        if (photo is null) return NotFound();
        else return Ok(photo);
    }
}
