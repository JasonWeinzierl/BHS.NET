using BHS.Contracts.Photos;
using BHS.DataAccess.Tests;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class AlbumRepositoryTests
    {
        private readonly AlbumRepository _subject;

        private readonly MockExecuter _mockExecuter = new();

        public AlbumRepositoryTests()
        {
            _subject = new AlbumRepository(_mockExecuter);
        }

        [Fact]
        public async Task GetById_Executes()
        {
            _mockExecuter.SingleResult = new Album(1, default, default, default, default, default);

            _ = await _subject.GetById(2);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Album_GetById", _mockExecuter.CommandText);

            Assert.Equal(2, _mockExecuter.Parameters.id);
        }

        [Fact]
        public async Task GetAll_Executes()
        {
            _mockExecuter.ManyResults = new Album[]
            {
                new Album(1, default, default, default, default, default)
            };

            _ = await _subject.GetAll();

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Album_GetAll", _mockExecuter.CommandText);
        }
    }
}
