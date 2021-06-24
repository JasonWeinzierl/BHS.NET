using BHS.Contracts.Leadership;
using BHS.DataAccess.Models;
using BHS.DataAccess.Tests;
using BHS.Model.Providers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class LeadershipRepositoryTests
    {
        private readonly LeadershipRepository _subject;

        private readonly MockExecuter _mockExecuter = new();
        private readonly Mock<IDateTimeOffsetProvider> _mockDtProvider;

        private readonly DateTimeOffset _someDate = new(2021, 03, 19, 00, 34, 00, TimeSpan.FromHours(-5));
        private readonly int _someYear = 2020;

        public LeadershipRepositoryTests()
        {
            _mockDtProvider = new Mock<IDateTimeOffsetProvider>(MockBehavior.Strict);
            _mockDtProvider.Setup(p => p.Now())
                .Returns(_someDate);
            _mockDtProvider.Setup(p => p.CurrentYear())
                .Returns(_someYear);

            _subject = new LeadershipRepository(_mockExecuter, _mockDtProvider.Object);
        }

        [Fact]
        public async Task GetCurrentOfficers_Executes()
        {
            _mockExecuter.ManyResults = new[] { new OfficerDto("x", "y", 1) };

            _ = await _subject.GetCurrentOfficers();

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("leadership.Officer_GetAll", _mockExecuter.CommandText);
        }

        [Fact]
        public async Task GetCurrentDirectors_Executes()
        {
            _mockExecuter.ManyResults = new[] { new DirectorDto("a", default) };

            _ = await _subject.GetCurrentDirectors();

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("leadership.Director_GetCurrent", _mockExecuter.CommandText);

            Assert.Equal(_someYear, _mockExecuter.Parameters?.startingYear);
        }
    }
}
