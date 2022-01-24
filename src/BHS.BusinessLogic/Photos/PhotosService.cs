using BHS.Contracts.Photos;
using BHS.Domain.Repositories;
using BHS.Domain.Services;
using Microsoft.Extensions.Logging;

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

        public Task<IReadOnlyCollection<Album>> GetAlbums()
            => _albumRepository.GetAll();

        public Task<AlbumPhotos?> GetAlbum(string slug)
            => _albumRepository.GetBySlug(slug);

        public Task<Photo?> GetPhoto(int id)
            => _photoRepository.GetById(id);
    }
}
