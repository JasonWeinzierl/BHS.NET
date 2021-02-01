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
        /// Get a photo
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Photo>> GetPhoto(int id)
        {
            var photo = await _photosService.GetPhoto(id);
            if (photo is null) return NotFound();
            return Ok(photo);
        }

        /// <summary>
        /// Get all albums
        /// </summary>
        [HttpGet("albums")]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            return Ok(await _photosService.GetAlbums());
        }

        /// <summary>
        /// Get an album
        /// </summary>
        [HttpGet("albums/{id}")]
        public async Task<ActionResult<Album>> GetAlbum(int id)
        {
            var album = await _photosService.GetAlbum(id);
            if (album is null) return NotFound();
            return Ok(album);
        }

        /// <summary>
        /// Get the photos of an album
        /// </summary>
        [HttpGet("albums/{id}/photos")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetAlbumPhotos(int id)
        {
            return Ok(await _photosService.GetPhotosByAlbum(id));
        }
    }
}
