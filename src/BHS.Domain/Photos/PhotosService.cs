using BHS.Contracts.Photos;
using Microsoft.Extensions.Logging;

namespace BHS.Domain.Photos;

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

    public Task<IReadOnlyCollection<Album>> GetAlbums(CancellationToken cancellationToken = default)
        => _albumRepository.GetAll(cancellationToken);

    public Task<AlbumPhotos?> GetAlbum(string slug, CancellationToken cancellationToken = default)
        => _albumRepository.GetBySlug(slug, cancellationToken);

    public Task<Photo?> GetPhoto(int id, CancellationToken cancellationToken = default)
        => _photoRepository.GetById(id, cancellationToken);
}
