using BHS.Contracts.Photos;
using BHS.DataAccess.Tests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PhotoRepositoryTests
    {
        private readonly PhotoRepository _subject;

        private readonly MockExecuter _mockExecuter = new();

        public PhotoRepositoryTests()
        {
            _subject = new PhotoRepository(_mockExecuter);
        }

        [Fact]
        public async Task GetById_Executes()
        {
            _mockExecuter.SingleResult = new Photo(9, "p", new Uri("scheme:path"), new DateTimeOffset(2020, 12, 16, 0, 5, 0, TimeSpan.FromHours(-6)), 8);

            _ = await _subject.GetById(2);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Photo_GetById", _mockExecuter.CommandText);

            Assert.Equal(2, _mockExecuter.Parameters.id);
        }

        [Fact]
        public async Task GetByAlbumId_Executes()
        {
            _mockExecuter.ManyResults = new Photo[]
            {
                new Photo(9, "p", new Uri("scheme:path"), new DateTimeOffset(2020, 12, 16, 0, 5, 0, TimeSpan.FromHours(-6)), 8)
            };

            _ = await _subject.GetByAlbumId(3);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Photo_GetByAlbumId", _mockExecuter.CommandText);

            Assert.Equal(3, _mockExecuter.Parameters.albumId);
        }
    }
}
