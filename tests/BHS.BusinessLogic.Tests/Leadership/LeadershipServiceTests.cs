using BHS.Model.DataAccess;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.BusinessLogic.Leadership.Tests
{
    public class LeadershipServiceTests
    {
        private readonly LeadershipService _subject;

        private readonly Mock<ILeadershipRepository> _mockLeadRepo;

        public LeadershipServiceTests()
        {
            _mockLeadRepo = new Mock<ILeadershipRepository>();

            _subject = new LeadershipService(_mockLeadRepo.Object);
        }

        [Fact]
        public async Task GetOfficers_CallsGetCurrentOfficers()
        {
            _ = await _subject.GetOfficers();

            _mockLeadRepo.Verify(r => r.GetCurrentOfficers(), Times.Once);
        }

        [Fact]
        public async Task GetDirectors_CallsGetCurrentDirectors()
        {
            _ = await _subject.GetDirectors();

            _mockLeadRepo.Verify(r => r.GetCurrentDirectors(), Times.Once);
        }
    }
}
