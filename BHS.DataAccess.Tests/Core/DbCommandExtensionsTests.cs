using BHS.DataAccess.Tests;
using Moq;
using System;
using System.Data;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class DbCommandExtensionsTests
    {
        private readonly MockDataSource MockData = new MockDataSource();

        [Fact]
        public void CreateParameter_CreatesParameter()
        {
            var mockCmd = MockData.CreateCommand();

            _ = mockCmd.Object.CreateParameter("@name", "value");

            mockCmd.Verify(c => c.CreateParameter(), Times.Once, "Expected CreateParameter to be called once.");
            Assert.Empty(MockData.Parameters);
        }

        [Fact]
        public void CreateParameter_AssignsArguments()
        {
            var mockCmd = MockData.CreateCommand();

            var result = mockCmd.Object.CreateParameter("@para", "value", DbType.String, ParameterDirection.Input);

            Assert.Equal("@para", result.ParameterName);
            Assert.Equal("value", result.Value);
            Assert.Equal(DbType.String, result.DbType);
            Assert.Equal(ParameterDirection.Input, result.Direction);
        }

        [Fact]
        public void CreateParameter_SetsDBNullIfNull()
        {
            var mockCmd = MockData.CreateCommand();

            var result = mockCmd.Object.CreateParameter("@param", null);

            Assert.NotNull(result.Value);
            Assert.Equal(DBNull.Value, result.Value);
        }
    }
}
