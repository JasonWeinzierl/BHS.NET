using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Tests;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class AlbumRepositoryTests
    {
        private readonly AlbumRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public AlbumRepositoryTests()
        {
            Subject = new AlbumRepository(new Querier(MockData.CreateDbConnectionFactory().Object));
        }

        [Fact]
        public async Task GetById_FillsResult()
        {
            var album = new Album(1, default, default, default, default, default, default, default);
            MockData.SetReaderResultset(new Album[] { album });

            var result = await Subject.GetById(0);

            Assert.NotNull(result);
            Assert.Equal(album.Id, result.Id);
            Assert.Equal(album.Name, result.Name);
            Assert.Equal(album.Description, result.Description);
            Assert.Equal(album.BannerPhotoId, result.BannerPhotoId);
            Assert.Equal(album.BlogPostId, result.BlogPostId);
            Assert.Equal(album.IsVisible, result.IsVisible);
            Assert.Equal(album.DateUpdated, result.DateUpdated);
            Assert.Equal(album.AuthorId, result.AuthorId);
        }

        [Fact]
        public async Task GetById_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetById(2);

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("photos.Album_GetById", MockData.CommandText);

            Assert.Equal("@id", MockData.Parameters[0].ParameterName);
            Assert.Equal(2, MockData.Parameters[0].Value);
        }

        [Fact]
        public async Task GetAll_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetAll().ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("photos.Album_GetAll", MockData.CommandText);

            Assert.Equal("@doIncludeHidden", MockData.Parameters[0].ParameterName);
            Assert.Equal(false, MockData.Parameters[0].Value);
        }
    }
}
