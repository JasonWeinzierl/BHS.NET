using BHS.Contracts.Photos;
using BHS.DataAccess.Models;
using BHS.DataAccess.Tests;
using System.Linq;
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
        public async Task GetBySlug_Executes()
        {
            string slug = "alb-2022";
            var album = new AlbumDto(
                slug,
                "some album",
                "some album desc",
                default,
                default,
                default,
                default,
                default,
                default,
                default,
                default,
                default);
            _mockExecuter.TwoManyResults = (new[] { album }, Enumerable.Empty<Photo>());

            _ = await _subject.GetBySlug(slug);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.AlbumPhotos_GetBySlug", _mockExecuter.CommandText);

            Assert.Equal(slug, _mockExecuter.Parameters?.slug);
        }

        [Fact]
        public async Task GetBySlug_JoinsMultipleResults()
        {
            string slug = "alb-2022";
            var album = new AlbumDto(
                slug,
                "some album",
                "some album desc",
                default,
                default,
                default,
                default,
                default,
                default,
                default,
                default,
                default);
            var photo1 = new Photo(1, default, default, default, default);
            var photo2 = new Photo(2, default, default, default, default);
            _mockExecuter.TwoManyResults = (new[] { album }, new[] { photo1, photo2 });

            var result = await _subject.GetBySlug(slug);

            Assert.NotNull(result);
            Assert.Equal(2, result?.Photos.Count());
        }

        [Fact]
        public async Task GetAll_Executes()
        {
            _mockExecuter.ManyResults = new AlbumDto[]
            {
                new AlbumDto(
                    "alb",
                    "some album",
                    "some album desc",
                    default,
                    default,
                    default,
                    default,
                    default,
                    default,
                    default,
                    default,
                    default)
            };

            _ = await _subject.GetAll();

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("photos.Album_GetAll", _mockExecuter.CommandText);
        }
    }
}
