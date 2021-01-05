using BHS.Contracts.Photos;
using BHS.DataAccess.Tests;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class AlbumRepositoryTests
    {
        private readonly AlbumRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();

        public AlbumRepositoryTests()
        {
            Subject = new AlbumRepository(MockQuerier);
        }

        [Fact]
        public async Task GetById_Executes()
        {
            MockQuerier.SingleResult = new Album(1, default, default, default, default, default);

            _ = await Subject.GetById(2);

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("photos.Album_GetById", MockQuerier.CommandText);

            Assert.Equal(2, MockQuerier.Parameters.id);
        }

        [Fact]
        public async Task GetAll_Executes()
        {
            MockQuerier.ManyResults = new Album[]
            {
                new Album(1, default, default, default, default, default)
            };

            _ = await Subject.GetAll();

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("photos.Album_GetAll", MockQuerier.CommandText);
        }
    }
}
