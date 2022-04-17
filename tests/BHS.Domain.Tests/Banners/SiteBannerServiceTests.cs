using BHS.Contracts.Banners;
using BHS.Domain.Banners;
using Moq;
using Xunit;

namespace BHS.Domain.Tests.Banners
{
    public class SiteBannerServiceTests
    {
        private readonly Mock<ISiteBannerRepository> _mockRepo = new(MockBehavior.Strict);

        private SiteBannerService Subject => new(_mockRepo.Object);

        private void VerifyAll() => Mock.Verify(_mockRepo);

        public class GetEnabled : SiteBannerServiceTests
        {
            [Fact]
            public async Task CallsRepo()
            {
                // Arrange
                _mockRepo
                    .Setup(repo => repo.GetEnabled())
                    .ReturnsAsync(Array.Empty<SiteBanner>());

                // Act
                var results = await Subject.GetEnabled();

                // Assert
                Assert.Empty(results);
                VerifyAll();
            }
        }
    }
}
