using BHS.Model.DataAccess;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.BusinessLogic.Photos.Tests
{
    public class PhotosServiceTests
    {
        private readonly PhotosService _subject;

        private readonly Mock<IPhotoRepository> _photoRepo;
        private readonly Mock<IAlbumRepository> _albumRepo;
        private readonly Mock<ILogger<PhotosService>> _logger;

        public PhotosServiceTests()
        {
            _photoRepo = new Mock<IPhotoRepository>();
            _albumRepo = new Mock<IAlbumRepository>();
            _logger = new Mock<ILogger<PhotosService>>();

            _subject = new PhotosService(_photoRepo.Object, _albumRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAlbums_CallsGetAll()
        {
            _ = await _subject.GetAlbums();

            _albumRepo.Verify(r => r.GetAll());
        }

        [Fact]
        public async Task GetAlbum_CallsGetBySlug()
        {
            string slug = "alb-2020";

            _ = await _subject.GetAlbum(slug);

            _albumRepo.Verify(r => r.GetBySlug(It.Is<string>(i => i == slug)));
        }

        [Fact]
        public async Task GetPhoto_CallsGetById()
        {
            int id = 9;

            _ = await _subject.GetPhoto(id);

            _photoRepo.Verify(r => r.GetById(It.Is<int>(i => i == id)));
        }
    }
}
