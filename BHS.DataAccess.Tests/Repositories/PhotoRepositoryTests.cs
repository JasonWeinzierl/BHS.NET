using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Tests;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PhotoRepositoryTests
    {
        private readonly PhotoRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public PhotoRepositoryTests()
        {
            Subject = new PhotoRepository(new Querier(MockData.CreateDbConnectionFactory().Object));
        }

        [Fact]
        public async Task GetById_FillsResult()
        {
            var photo = new Photo(9, "p", new Uri("scheme:path"), true, new DateTimeOffset(2020, 12, 16, 0, 5, 0, TimeSpan.FromHours(-6)), 8);
            MockData.SetReaderResultset(new Photo[] { photo });

            var result = await Subject.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(photo.Id, result.Id);
            Assert.Equal(photo.Name, result.Name);
            Assert.Equal(photo.ImageUri, result.ImageUri);
            Assert.Equal(photo.IsVisible, result.IsVisible);
            Assert.Equal(photo.DatePosted, result.DatePosted);
            Assert.Equal(photo.AuthorId, result.AuthorId);
        }

        [Fact]
        public async Task GetById_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetById(2);

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("photos.Photo_GetById", MockData.CommandText);

            Assert.Equal("@id", MockData.Parameters[0].ParameterName);
            Assert.Equal(2, MockData.Parameters[0].Value);
        }

        [Fact]
        public async Task GetByAlbumId_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetByAlbumId(3).ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("photos.Photo_GetByAlbumId", MockData.CommandText);

            Assert.Equal("@albumId", MockData.Parameters[0].ParameterName);
            Assert.Equal(3, MockData.Parameters[0].Value);
        }
    }
}
