using BHS.Contracts.Photos;
using BHS.DataAccess.Tests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PhotoRepositoryTests
    {
        private readonly PhotoRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();

        public PhotoRepositoryTests()
        {
            Subject = new PhotoRepository(MockQuerier);
        }

        [Fact]
        public async Task GetById_Executes()
        {
            MockQuerier.SingleResult = new Photo(9, "p", new Uri("scheme:path"), true, new DateTimeOffset(2020, 12, 16, 0, 5, 0, TimeSpan.FromHours(-6)), 8);

            _ = await Subject.GetById(2);

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("photos.Photo_GetById", MockQuerier.CommandText);

            Assert.Equal(2, MockQuerier.Parameters.id);
        }

        [Fact]
        public async Task GetByAlbumId_Executes()
        {
            MockQuerier.ManyResults = new Photo[]
            {
                new Photo(9, "p", new Uri("scheme:path"), true, new DateTimeOffset(2020, 12, 16, 0, 5, 0, TimeSpan.FromHours(-6)), 8)
            };

            _ = await Subject.GetByAlbumId(3);

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("photos.Photo_GetByAlbumId", MockQuerier.CommandText);

            Assert.Equal(3, MockQuerier.Parameters.albumId);
        }
    }
}
