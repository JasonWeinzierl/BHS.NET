using BHS.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data.Common;
using Xunit;

namespace BHS.Infrastructure.Tests.Core
{
    public class SqlConnectionFactoryTests
    {
        private readonly SqlConnectionFactory _subject;

        private string? ConnectionStringName;

        public SqlConnectionFactoryTests()
        {
            const string connectionStringsSectionName = "ConnectionStrings";
            var inMemoryData = new Dictionary<string, string>
            {
                { $"{connectionStringsSectionName}:db", "mock connection string" },
            };

            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryData)
                .Build();

            _subject = new SqlConnectionFactory(inMemoryConfig, null);

            var mockConnection = new Mock<DbConnection>(MockBehavior.Strict);
            mockConnection.SetupSet(c => c.ConnectionString = It.IsAny<string>())
                .Callback<string>(connStrName => ConnectionStringName = connStrName);
            mockConnection.As<IDisposable>().Setup(d => d.Dispose());

            var mockPrvdrFctry = new Mock<DbProviderFactory>(MockBehavior.Strict);
            mockPrvdrFctry.Setup(f => f.CreateConnection())
                .Returns(mockConnection.Object);

            DbProviderFactories.RegisterFactory(DbConstants.SqlClientProviderName, mockPrvdrFctry.Object);
        }

        [Fact]
        public void CreateConnection_Happy()
        {
            var result = _subject.CreateConnection("db");

            Assert.NotNull(result);
            Assert.Equal("mock connection string", ConnectionStringName);
        }

        [Fact]
        public void CreateConnection_OnMissingConnectionString_Throws()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = _subject.CreateConnection("missingdb");
            });
        }
    }
}
