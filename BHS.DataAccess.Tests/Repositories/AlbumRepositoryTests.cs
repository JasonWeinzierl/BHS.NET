using BHS.Contracts.Photos;
using BHS.DataAccess.Models;
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
            int id = 2;
            _mockExecuter.SingleResult = new AlbumDTO(id, default, default, default, default, default);

            _ = await _subject.GetById(id);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Album_GetById", _mockExecuter.CommandText);

            Assert.Equal(id, _mockExecuter.Parameters?.id);
        }

        [Fact]
        public async Task GetAll_Executes()
        {
            _mockExecuter.ManyResults = new AlbumDTO[]
            {
                new AlbumDTO(1, default, default, default, default, default)
            };

            _ = await _subject.GetAll();

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Album_GetAll", _mockExecuter.CommandText);
        }
    }
}
