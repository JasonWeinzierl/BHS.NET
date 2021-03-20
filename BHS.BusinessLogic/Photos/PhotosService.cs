using BHS.Contracts.Photos;
using BHS.Model.DataAccess;
using BHS.Model.Services.Photos;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Photos
{
    public class PhotosService : IPhotosService
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly ILogger _logger;

        public PhotosService(
            IPhotoRepository photoRepository,
            IAlbumRepository albumRepository,
            ILogger<PhotosService> logger)
        {
            _photoRepository = photoRepository;
            _albumRepository = albumRepository;
            _logger = logger;
        }

        public Task<IEnumerable<Album>> GetAlbums()
        {
            return _albumRepository.GetAll();
        }

        public Task<Album?> GetAlbum(string slug)
        {
            // TODO: convert the underlying table to use slug. This is a temp fix.
            if (int.TryParse(slug, out int id))
                return _albumRepository.GetById(id);
            else
                return Task.FromResult<Album?>(null);
        }

        public Task<IEnumerable<Photo>> GetPhotosByAlbum(string slug)
        {
            if (int.TryParse(slug, out int id))
                return _photoRepository.GetByAlbumId(id);
            else
                return Task.FromResult(Enumerable.Empty<Photo>());
        }

        public Task<Photo?> GetPhoto(int id)
        {
            return _photoRepository.GetById(id);
        }
    }
}
