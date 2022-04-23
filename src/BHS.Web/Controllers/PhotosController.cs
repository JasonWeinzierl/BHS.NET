using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
    private readonly IPhotosService _photosService;

    public PhotosController(IPhotosService photosService)
    {
        _photosService = photosService;
    }

    /// <summary>
    /// Gets a photo.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Photo))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Photo>> GetPhoto(int id)
    {
        var photo = await _photosService.GetPhoto(id);
        if (photo is null) return NotFound();
        else return Ok(photo);
    }

    /// <summary>
    /// Gets all albums.
    /// </summary>
    [HttpGet("albums")]
    public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        => Ok(await _photosService.GetAlbums());

    /// <summary>
    /// Gets an album and its photos.
    /// </summary>
    [HttpGet("albums/{slug}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlbumPhotos))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlbumPhotos>> GetAlbum(string slug)
    {
        var album = await _photosService.GetAlbum(slug);
        if (album is null) return NotFound();
        else return Ok(album);
    }
}
