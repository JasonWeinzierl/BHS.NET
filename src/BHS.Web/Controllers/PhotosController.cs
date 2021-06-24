using BHS.Contracts.Photos;
using BHS.Model.Services.Photos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Web.Controllers
{
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
        /// Get a photo.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            var photo = await _photosService.GetPhoto(id);
            if (photo is null)
                return NotFound();
            return Ok(photo);
        }

        /// <summary>
        /// Get all albums.
        /// </summary>
        [HttpGet("albums")]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
            => Ok(await _photosService.GetAlbums());

        /// <summary>
        /// Get an album and its photos.
        /// </summary>
        [HttpGet("albums/{slug}")]
        public async Task<ActionResult<AlbumPhotos>> GetAlbum(string slug)
        {
            var album = await _photosService.GetAlbum(slug);
            if (album is null)
                return NotFound();
            return Ok(album);
        }
    }
}
