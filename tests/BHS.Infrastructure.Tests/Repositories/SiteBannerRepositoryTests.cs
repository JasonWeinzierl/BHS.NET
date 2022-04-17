using BHS.Infrastructure.Models;
using BHS.Infrastructure.Repositories;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories
{
    public class SiteBannerRepositoryTests
    {
        private readonly SiteBannerRepository _subject;

        private readonly MockExecuter _mockExecuter = new();

        public SiteBannerRepositoryTests()
        {
            _subject = new(_mockExecuter);
        }

        public class GetEnabled : SiteBannerRepositoryTests
        {
            [Fact]
            public async Task Executes()
            {
                // Arrange
                var expected = new SiteBannerDTO(1, 2, "lead", null);
                _mockExecuter.ManyResults = new[] { expected };

                // Act
                var results = await _subject.GetEnabled();

                // Assert
                Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
                Assert.Equal("[banners].[SiteBanner_GetEnabled]", _mockExecuter.CommandText);

                var result = Assert.Single(results);
                Assert.Equal(expected.ToDomainModel(), result);
            }
        }
    }
}
