using BHS.Contracts;
using BHS.DataAccess.Tests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class ContactAlertRepositoryTests
    {
        private readonly ContactAlertRepository _subject;

        private readonly MockExecuter _mockExecuter = new();

        public ContactAlertRepositoryTests()
        {
            _subject = new ContactAlertRepository(_mockExecuter);
        }

        [Fact]
        public async Task Insert_Executes()
        {
            var request = new ContactAlertRequest("z", "x", "c", new DateTimeOffset(2020, 12, 16, 0, 0, 0, TimeSpan.FromHours(-6)), default);
            _mockExecuter.SingleResult = new ContactAlert(default, default, default, default, default, default);

            _ = await _subject.Insert(request);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("dbo.ContactAlert_Insert", _mockExecuter.CommandText);

            Assert.Equal(request.Name, _mockExecuter.Parameters.name);
            Assert.Equal(request.EmailAddress, _mockExecuter.Parameters.emailAddress);
            Assert.Equal(request.Message, _mockExecuter.Parameters.message);
            Assert.Equal(request.DateRequested, _mockExecuter.Parameters.dateRequested);
        }
    }
}
