using BHS.DataAccess.Tests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    // todo: maybe directly extend from sproc repository base to test protected methods
    public class SprocRepositoryBaseTests
    {
        private readonly TestRepository testRepo;
        private readonly MockDataSource MockData = new MockDataSource();

        public SprocRepositoryBaseTests()
        {
            testRepo = new TestRepository(MockData.CreateDbConnectionFactory().Object);
        }

        [Fact]
        public async Task ExecuteReaderAsync_ReadsAllData()
        {
            var table = new DataTable();
            table.Columns.Add("Col1");
            table.Rows.Add("val1");
            table.Rows.Add(DBNull.Value);
            MockData.ReaderResultset = table;

            var result = await testRepo.ReadResultset().ToListAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ToDataTable_ConvertsListOfInts()
        {
            var ints = new int[] { 1, 0, -1 };

            var result = TestRepository.CreateDataTable(ints);

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal(1, result.Rows[0]["Number"]);
            Assert.Equal(0, result.Rows[1]["Number"]);
            Assert.Equal(-1, result.Rows[2]["Number"]);
        }

        [Fact]
        public void ToDataTable_ConvertsListOfNullableInts()
        {
            var ints = new int?[] { -1, 0, null };

            var result = TestRepository.CreateDataTable(ints);

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal(-1, result.Rows[0]["Number"]);
            Assert.Equal(0, result.Rows[1]["Number"]);
            Assert.Equal(DBNull.Value, result.Rows[2]["Number"]);
        }

        [Fact]
        public void ToDataTable_ConvertsListOfStrings()
        {
            var strings = new string[] { "A", string.Empty, null };

            var result = TestRepository.CreateDataTable(strings);

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal("A", result.Rows[0]["String"]);
            Assert.Equal(string.Empty, result.Rows[1]["String"]);
            Assert.Equal(DBNull.Value, result.Rows[2]["String"]);
        }

        // todo: consider why these 4 datatable converters.  why no nullable long?
        [Fact]
        public void ToDataTable_ConvertsListOfLongs()
        {
            var strings = new long[] { int.MaxValue + 1L, 0L, long.MinValue };

            var result = TestRepository.CreateDataTable(strings);

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal(int.MaxValue + 1L, result.Rows[0]["Long"]);
            Assert.Equal(0L, result.Rows[1]["Long"]);
            Assert.Equal(long.MinValue, result.Rows[2]["Long"]);
        }

        [Fact]
        public async Task ExecuteScalarAsync_ReturnsResult()
        {
            int i = 1;
            MockData.ScalarCell = i;

            var result = await testRepo.ReadScalar();

            Assert.Equal(i, result);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_RunsQuery()
        {
            MockData.NonQueryRowsAffected = 1;

            int affected = await testRepo.GetNumAffected();

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

            var result = await testRepo.GetOutputParams();

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Process", MockData.CommandText);

            Assert.Equal("Value", result);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            Assert.Equal("@para", MockData.Parameters[0].ParameterName);
            Assert.Equal("value", MockData.Parameters[0].Value);
            // todo: verify NonQuery was called
        }

        // todo: ensure this exposes all methods
        // or alternatively consider rewriting sprocrepositorybase to be a dependency
        // instead of an abstract base?  separate dependency for the converters?
        public class TestRepository : SprocRepositoryBase
        {
            public TestRepository(IDbConnectionFactory factory) : base(factory) { }

            public IAsyncEnumerable<string> ReadResultset()
            {
                return ExecuteReaderAsync("connstr", "Get", null, dr => "Value");
            }

            public Task<int> ReadScalar()
            {
                return ExecuteScalarAsync<int>("connstr", "Insert", null);
            }

            public Task<int> GetNumAffected()
            {
                return ExecuteNonQueryAsync("connstr", "Save", cmd =>
                {
                    cmd.Parameters.Add(new MockDbDataParameter { ParameterName = "@para", Value = "value" });
                });
            }

            public Task<string> GetOutputParams()
            {
                return ExecuteNonQueryAsync("connstr", "Process", cmd =>
                {
                    cmd.Parameters.Add(new MockDbDataParameter { ParameterName = "@para", Value = "value" });
                }, c => "Value");
            }

            public static DataTable CreateDataTable(IEnumerable<int> ints) => ToDataTable(ints);
            public static DataTable CreateDataTable(IEnumerable<int?> nullableInts) => ToDataTable(nullableInts);
            public static DataTable CreateDataTable(IEnumerable<string> strings) => ToDataTable(strings);
            public static DataTable CreateDataTable(IEnumerable<long> longs) => ToDataTable(longs);
        }
    }
}
