using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.DataAccess.Tests;
using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class ContactAlertRepositoryTests
    {
        private readonly ContactAlertRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public ContactAlertRepositoryTests()
        {
            Subject = new ContactAlertRepository(new Querier(MockData.CreateDbConnectionFactory().Object));
        }

        [Fact]
        public async Task Insert_FillsResult()
        {
            var alert = new ContactAlert(1, "q", "w", "e", new DateTimeOffset(2020, 12, 15, 23, 53, 0, TimeSpan.FromHours(-6)), new DateTimeOffset(2020, 12, 15, 23, 55, 0, TimeSpan.FromHours(-6)));
            MockData.SetReaderResultset(new ContactAlert[] { alert });

            var result = await Subject.Insert(new ContactAlertRequest(default, default, default, default, default));

            Assert.NotNull(result);
            Assert.Equal(alert.Id, result.Id);
            Assert.Equal(alert.Name, result.Name);
            Assert.Equal(alert.EmailAddress, result.EmailAddress);
            Assert.Equal(alert.Message, result.Message);
            Assert.Equal(alert.DateRequested, result.DateRequested);
            Assert.Equal(alert.DateCreated, result.DateCreated);
        }

        [Fact]
        public async Task Insert_Command()
        {
            MockData.ReaderResultset = new DataTable();
            var request = new ContactAlertRequest("z", "x", "c", new DateTimeOffset(2020, 12, 16, 0, 0, 0, TimeSpan.FromHours(-6)), default);

            _ = await Subject.Insert(request);

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("dbo.ContactAlert_Insert", MockData.CommandText);

            Assert.Equal("@name", MockData.Parameters[0].ParameterName);
            Assert.Equal(request.Name, MockData.Parameters[0].Value);
            Assert.Equal("@emailAddress", MockData.Parameters[1].ParameterName);
            Assert.Equal(request.EmailAddress, MockData.Parameters[1].Value);
            Assert.Equal("@message", MockData.Parameters[2].ParameterName);
            Assert.Equal(request.Message, MockData.Parameters[2].Value);
            Assert.Equal("@dateRequested", MockData.Parameters[3].ParameterName);
            Assert.Equal(request.DateRequested, MockData.Parameters[3].Value);
        }
    }
}
