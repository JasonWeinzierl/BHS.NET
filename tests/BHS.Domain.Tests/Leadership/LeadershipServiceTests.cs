using BHS.Contracts.Leadership;
using BHS.Domain.Leadership;
using Moq;
using Xunit;

namespace BHS.Domain.Tests.Leadership
{
    public class LeadershipServiceTests
    {
        private readonly LeadershipService _subject;

        private readonly Mock<ILeadershipRepository> _mockLeadRepo;

        public LeadershipServiceTests()
        {
            _mockLeadRepo = new Mock<ILeadershipRepository>(MockBehavior.Strict);

            _subject = new LeadershipService(_mockLeadRepo.Object);
        }

        [Fact]
        public async Task GetOfficers_CallsGetCurrentOfficers()
        {
            _mockLeadRepo.Setup(r => r.GetCurrentOfficers())
                .ReturnsAsync(Array.Empty<Officer>());

            var result = await _subject.GetOfficers();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDirectors_CallsGetCurrentDirectors()
        {
            _mockLeadRepo.Setup(r => r.GetCurrentDirectors())
                .ReturnsAsync(Array.Empty<Director>());

            var result = await _subject.GetDirectors();

            Assert.Empty(result);
        }
    }
}
