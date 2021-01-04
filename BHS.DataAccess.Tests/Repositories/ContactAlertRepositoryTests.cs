using BHS.Contracts;
using BHS.DataAccess.Tests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class ContactAlertRepositoryTests
    {
        private readonly ContactAlertRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();

        public ContactAlertRepositoryTests()
        {
            Subject = new ContactAlertRepository(MockQuerier);
        }

        [Fact]
        public async Task Insert_Executes()
        {
            var request = new ContactAlertRequest("z", "x", "c", new DateTimeOffset(2020, 12, 16, 0, 0, 0, TimeSpan.FromHours(-6)), default);
            MockQuerier.SingleResult = new ContactAlert(default, default, default, default, default, default);

            _ = await Subject.Insert(request);

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("dbo.ContactAlert_Insert", MockQuerier.CommandText);

            Assert.Equal(request.Name, MockQuerier.Parameters.name);
            Assert.Equal(request.EmailAddress, MockQuerier.Parameters.emailAddress);
            Assert.Equal(request.Message, MockQuerier.Parameters.message);
            Assert.Equal(request.DateRequested, MockQuerier.Parameters.dateRequested);
        }
    }
}
