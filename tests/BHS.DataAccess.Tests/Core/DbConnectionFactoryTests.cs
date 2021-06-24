using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class DbConnectionFactoryTests
    {
        private readonly DbConnectionFactory _subject;

        private string? ConnectionStringName;

        public DbConnectionFactoryTests()
        {
            const string connectionStringsSectionName = "ConnectionStrings";
            var inMemoryData = new Dictionary<string, string>
            {
                { $"{connectionStringsSectionName}:db", "mock connection string" },
                { $"{connectionStringsSectionName}:db_ProviderName", "MockDbProviderFactory" },
                { $"{connectionStringsSectionName}:missingprovider", "mock connection string" },
                { $"{connectionStringsSectionName}:badProviderName", "mock connection string" },
                { $"{connectionStringsSectionName}:badProviderName_ProviderName", "BadDbProviderFactory" }
            };

            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryData)
                .Build();

            _subject = new DbConnectionFactory(inMemoryConfig, null);

            var mockConnection = new Mock<DbConnection>(MockBehavior.Strict);
            mockConnection.SetupSet(c => c.ConnectionString = It.IsAny<string>())
                .Callback<string>(connStrName => ConnectionStringName = connStrName);

            var mockPrvdrFctry = new Mock<DbProviderFactory>(MockBehavior.Strict);
            mockPrvdrFctry.Setup(f => f.CreateConnection())
                .Returns(mockConnection.Object);

            DbProviderFactories.RegisterFactory("MockDbProviderFactory", mockPrvdrFctry.Object);
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

        [Fact]
        public void CreateConnection_OnMissingProviderName_Throws()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = _subject.CreateConnection("missingprovider");
            });
        }

        [Fact]
        public void CreateConnection_OnProviderFactoryNotRegistered_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = _subject.CreateConnection("badProviderName");
            });
        }
    }
}
