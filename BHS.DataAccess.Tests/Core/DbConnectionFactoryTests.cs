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
        private readonly DbConnectionFactory Subject;

        private string ConnectionStringName;

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

            Subject = new DbConnectionFactory(inMemoryConfig, null);

            var mockConnection = new Mock<DbConnection>();
            mockConnection.SetupSet(c => c.ConnectionString = It.IsAny<string>())
                .Callback<string>(connStrName => ConnectionStringName = connStrName);

            var mockPrvdrFctry = new Mock<DbProviderFactory>();
            mockPrvdrFctry.Setup(f => f.CreateConnection())
                .Returns(mockConnection.Object);

            DbProviderFactories.RegisterFactory("MockDbProviderFactory", mockPrvdrFctry.Object);
        }

        [Fact]
        public void CreateConnection_Happy()
        {
            var result = Subject.CreateConnection("db");

            Assert.NotNull(result);
            Assert.Equal("mock connection string", ConnectionStringName);
        }

        [Fact]
        public void CreateConnection_ThrowsOnMissingConnectionString()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Subject.CreateConnection("missingdb");
            });
        }

        [Fact]
        public void CreateConnection_ThrowsOnMissingProviderName()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = Subject.CreateConnection("missingprovider");
            });
        }

        [Fact]
        public void CreateConnection_ThrowsOnProviderFactoryNotRegistered()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = Subject.CreateConnection("badProviderName");
            });
        }
    }
}
