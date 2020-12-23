using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class SprocRepositoryBaseTests
    {
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

        public class TestRepository : SprocRepositoryBase
        {
            public TestRepository(IQuerier querier) : base(querier) { }

            public static DataTable CreateDataTable(IEnumerable<int> ints) => ToDataTable(ints);
            public static DataTable CreateDataTable(IEnumerable<int?> nullableInts) => ToDataTable(nullableInts);
            public static DataTable CreateDataTable(IEnumerable<string> strings) => ToDataTable(strings);
            public static DataTable CreateDataTable(IEnumerable<long> longs) => ToDataTable(longs);
        }
    }
}
