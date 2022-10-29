using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BHS.Domain.Tests.Photos;

public class PhotosServiceTests
{
    private readonly PhotosService _subject;

    private readonly Mock<IPhotoRepository> _photoRepo;
    private readonly Mock<IAlbumRepository> _albumRepo;
    private readonly Mock<ILogger<PhotosService>> _logger;

    public PhotosServiceTests()
    {
        _photoRepo = new Mock<IPhotoRepository>(MockBehavior.Strict);
        _albumRepo = new Mock<IAlbumRepository>(MockBehavior.Strict);
        _logger = new Mock<ILogger<PhotosService>>();

        _subject = new PhotosService(_photoRepo.Object, _albumRepo.Object, _logger.Object);
    }

    [Fact]
    public async Task GetAlbums_CallsGetAll()
    {
        _albumRepo
            .Setup(r => r.GetAll(default))
            .ReturnsAsync(Array.Empty<Album>());

        var result = await _subject.GetAlbums();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAlbum_CallsGetBySlug()
    {
        string slug = "alb-2020";
        _albumRepo
            .Setup(r => r.GetBySlug(slug, default))
            .ReturnsAsync((AlbumPhotos?)null);

        var result = await _subject.GetAlbum(slug);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPhoto_CallsGetById()
    {
        int id = 9;
        _photoRepo
            .Setup(r => r.GetById(id, default))
            .ReturnsAsync((Photo?)null);

        var result = await _subject.GetPhoto(id);

        Assert.Null(result);
    }
}
