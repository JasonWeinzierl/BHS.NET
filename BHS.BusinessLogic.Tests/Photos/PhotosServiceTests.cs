using BHS.Model.DataAccess;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.BusinessLogic.Photos.Tests
{
    public class PhotosServiceTests
    {
        private readonly PhotosService Subject;

        private readonly Mock<IPhotoRepository> _photoRepo;
        private readonly Mock<IAlbumRepository> _albumRepo;
        private readonly Mock<ILogger<PhotosService>> _logger;

        public PhotosServiceTests()
        {
            _photoRepo = new Mock<IPhotoRepository>();
            _albumRepo = new Mock<IAlbumRepository>();
            _logger = new Mock<ILogger<PhotosService>>();

            Subject = new PhotosService(_photoRepo.Object, _albumRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAlbums_CallsGetAll()
        {
            _ = await Subject.GetAlbums();

            _albumRepo.Verify(r => r.GetAll());
        }

        [Fact]
        public async Task GetAlbum_CallsGetById()
        {
            int id = 5;

            _ = await Subject.GetAlbum(id.ToString());

            _albumRepo.Verify(r => r.GetById(It.Is<int>(i => i == id)));
        }

        [Fact]
        public async Task GetPhotosByAlbum_CallsGetByAlbumId()
        {
            int id = 6;

            _ = await Subject.GetPhotosByAlbum(id.ToString());

            _photoRepo.Verify(r => r.GetByAlbumId(It.Is<int>(i => i == id)));
        }

        [Fact]
        public async Task GetPhoto_CallsGetById()
        {
            int id = 9;

            _ = await Subject.GetPhoto(id);

            _photoRepo.Verify(r => r.GetById(It.Is<int>(i => i == id)));
        }
    }
}
