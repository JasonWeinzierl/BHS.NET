using System;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class EnumerableToTVPExtensionsTests
    {
        [Fact]
        public void ToDataTable_ConvertsListOfInts()
        {
            var ints = new int[] { 1, 0, -1 };

            var result = EnumerableToTVPExtensions.ToDataTable(ints, "Number");

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal(1, result.Rows[0]["Number"]);
            Assert.Equal(0, result.Rows[1]["Number"]);
            Assert.Equal(-1, result.Rows[2]["Number"]);
        }

        [Fact]
        public void ToDataTable_ConvertsListOfNullableInts()
        {
            var ints = new int?[] { -1, 0, null };

            var result = EnumerableToTVPExtensions.ToDataTable(ints, "Number");

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal(-1, result.Rows[0]["Number"]);
            Assert.Equal(0, result.Rows[1]["Number"]);
            Assert.Equal(DBNull.Value, result.Rows[2]["Number"]);
        }

        [Fact]
        public void ToDataTable_ConvertsListOfStrings()
        {
            var strings = new string?[] { "A", string.Empty, null };

            var result = EnumerableToTVPExtensions.ToDataTable(strings, "String");

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal("A", result.Rows[0]["String"]);
            Assert.Equal(string.Empty, result.Rows[1]["String"]);
            Assert.Equal(DBNull.Value, result.Rows[2]["String"]);
        }

        [Fact]
        public void ToDataTable_ConvertsListOfLongs()
        {
            var longs = new long[] { int.MaxValue + 1L, 0L, long.MinValue };

            var result = EnumerableToTVPExtensions.ToDataTable(longs, "Long");

            Assert.Equal(3, result.Rows.Count);
            Assert.Equal(int.MaxValue + 1L, result.Rows[0]["Long"]);
            Assert.Equal(0L, result.Rows[1]["Long"]);
            Assert.Equal(long.MinValue, result.Rows[2]["Long"]);
        }
    }
}
