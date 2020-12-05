using BHS.Contracts.Photos;
using BHS.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

        public Task<IEnumerable<Album>> GetAlbums(bool doIncludeHidden = false)
        {
            return _albumRepository.GetAll(doIncludeHidden);
        }

        public Task<Album> GetAlbum(int id)
        {
            return _albumRepository.GetById(id);
        }

        public Task<IEnumerable<Photo>> GetPhotosByAlbum(int id)
        {
            return _photoRepository.GetByAlbumId(id);
        }

        public Task<Photo> GetPhoto(int id)
        {
            return _photoRepository.GetById(id);
        }
    }
}
