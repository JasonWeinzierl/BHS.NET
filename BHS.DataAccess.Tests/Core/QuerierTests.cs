using BHS.DataAccess.Tests;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    // todo: coverage
    public class QuerierTests
    {
        private readonly Querier Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public QuerierTests()
        {
            Subject = new Querier(MockData.CreateDbConnectionFactory().Object);
        }

        [Fact]
        public async Task ExecuteReaderAsync_ReadsAllData()
        {
            var table = new DataTable();
            table.Columns.Add("Col1");
            table.Rows.Add("val1");
            table.Rows.Add(DBNull.Value);
            MockData.ReaderResultset = table;

            var result = await Subject.ExecuteReaderAsync("connstr", "Get", null, dr => "Value").ToListAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            //todo
        }

        [Fact]
        public async Task ExecuteScalarAsync_ReturnsResult()
        {
            int i = 1;
            MockData.ScalarCell = i;

            var result = await Subject.ExecuteScalarAsync<int>("connstr", "Insert", null);

            Assert.Equal(i, result);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_RunsQuery()
        {
            MockData.NonQueryRowsAffected = 1;

            int affected = await Subject.ExecuteNonQueryAsync("connstr", "Save", cmd =>
            {
                cmd.Parameters.Add(new MockDbDataParameter { ParameterName = "@para", Value = "value" });
            });

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Save", MockData.CommandText);

            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            Assert.Equal("@para", MockData.Parameters[0].ParameterName);
            Assert.Equal("value", MockData.Parameters[0].Value);
            Assert.Equal(1, affected);
            // todo: verify NonQuery was called
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_OutputParameters()
        {
            MockData.NonQueryRowsAffected = 1;

            var result = await Subject.ExecuteNonQueryAsync("connstr", "Process", cmd =>
            {
                cmd.Parameters.Add(new MockDbDataParameter { ParameterName = "@para", Value = "value" });
            }, c => "Value");

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Process", MockData.CommandText);

            Assert.Equal("Value", result);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            Assert.Equal("@para", MockData.Parameters[0].ParameterName);
            Assert.Equal("value", MockData.Parameters[0].Value);
            // todo: verify NonQuery was called
        }
    }
}
