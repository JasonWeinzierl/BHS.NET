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
        private readonly EachPrimitiveType someModel;

        public SprocRepositoryBaseTests()
        {
            testRepo = new TestRepository(MockData.CreateDbConnectionFactory().Object);
            someModel = new EachPrimitiveType(
                int.MaxValue + 1L,
                int.MinValue - 1L,
                short.MaxValue + 1,
                short.MinValue - 1,
                byte.MaxValue + 1,
                byte.MinValue - 1,
                255,
                0,
                1.1F,
                2.2F,
                3.3M,
                4.4M,
                "querty",
                'x',
                '\0',
                new DateTimeOffset(2020, 12, 16, 18, 35, 0, TimeSpan.FromHours(-6)),
                new DateTimeOffset(2020, 12, 16, 18, 36, 0, TimeSpan.FromHours(-6)),
                true,
                true
                );
        }

        [Fact]
        public async Task ExecuteReaderAsync_ReadsAllData()
        {
            var table = MockDataSource.CreateResultset(new EachPrimitiveType[] { someModel });
            var row1null = table.NewRow();
            table.Rows.Add(row1null);
            foreach (DataColumn c in table.Columns)
                row1null[c.ColumnName] = DBNull.Value;
            MockData.ReaderResultset = table;

            var result = await testRepo.ReadResultset().ToListAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(someModel.LongNumber, result[0].LongNumber);
            Assert.Equal(someModel.NullableLongNumber, result[0].NullableLongNumber);
            Assert.Equal(someModel.Integer, result[0].Integer);
            Assert.Equal(someModel.NullableInteger, result[0].NullableInteger);
            Assert.Equal(someModel.Short, result[0].Short);
            Assert.Equal(someModel.NullableShort, result[0].NullableShort);
            Assert.Equal(someModel.Byte, result[0].Byte);
            Assert.Equal(someModel.NullableByte, result[0].NullableByte);
            Assert.Equal(someModel.DoubleNumber, result[0].DoubleNumber);
            Assert.Equal(someModel.NullableDoubleNumber, result[0].NullableDoubleNumber);
            Assert.Equal(someModel.DecimalNumber, result[0].DecimalNumber);
            Assert.Equal(someModel.NullableDecimalNumber, result[0].NullableDecimalNumber);
            Assert.Equal(someModel.String, result[0].String);
            Assert.Equal(someModel.Character, result[0].Character);
            Assert.Equal(someModel.NullableCharacter, result[0].NullableCharacter);
            Assert.Equal(someModel.Boolean, result[0].Boolean);
            Assert.Equal(someModel.NullableBoolean, result[0].NullableBoolean);

            var utcDate = new DateTimeOffset(2020, 12, 17, 0, 35, 0, TimeSpan.FromHours(0));
            Assert.Equal(utcDate, result[0].Date);

            var nullableUtcDate = new DateTimeOffset(2020, 12, 17, 0, 36, 0, TimeSpan.FromHours(0));
            Assert.Equal(nullableUtcDate, result[0].NullableDate);

            Assert.Equal(default, result[1].LongNumber);
            Assert.Equal(default, result[1].NullableLongNumber);
            Assert.Equal(default, result[1].Integer);
            Assert.Equal(default, result[1].NullableInteger);
            Assert.Equal(default, result[1].Short);
            Assert.Equal(default, result[1].NullableShort);
            Assert.Equal(default, result[1].Byte);
            Assert.Equal(default, result[1].NullableByte);
            Assert.True(double.IsNaN(result[1].DoubleNumber));
            Assert.Equal(default, result[1].NullableDoubleNumber);
            Assert.Equal(default, result[1].DecimalNumber);
            Assert.Equal(default, result[1].NullableDecimalNumber);
            Assert.Equal(default, result[1].String);
            Assert.Equal(default, result[1].Character);
            Assert.Equal(default, result[1].NullableCharacter);
            Assert.Equal(default, result[1].Date);
            Assert.Equal(default, result[1].NullableDate);
            Assert.Equal(default, result[1].Boolean);
            Assert.Equal(default, result[1].NullableBoolean);
        }

        [Fact]
        public async Task ToBool_ReadsInt()
        {
            var table = MockDataSource.CreateResultset<EachPrimitiveType>();
            table.Columns["Boolean"].DataType = typeof(int);
            var row = table.NewRow();
            table.Rows.Add(row);
            row["Boolean"] = 1;
            row = table.NewRow();
            table.Rows.Add(row);
            row["Boolean"] = 0;
            MockData.ReaderResultset = table;

            var result = await testRepo.ReadResultset().ToListAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.True(result[0].Boolean);
            Assert.False(result[1].Boolean);
        }

        [Fact]
        public Task ToBool_ThrowsOnInvalidInt()
        {
            var table = MockDataSource.CreateResultset<EachPrimitiveType>();
            table.Columns["Boolean"].DataType = typeof(int);
            var row = table.NewRow();
            table.Rows.Add(row);
            row["Boolean"] = 3;
            MockData.ReaderResultset = table;

            return Assert.ThrowsAsync<InvalidCastException>(async () =>
            {
                _ = await testRepo.ReadResultset().ToListAsync();
            });
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

            public IAsyncEnumerable<EachPrimitiveType> ReadResultset()
            {
                return ExecuteReaderAsync("connstr", "Get", null, Get);
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

            public static EachPrimitiveType Get(IDataRecord dr)
            {
                return new EachPrimitiveType(
                    ToLong(dr["LongNumber"]),
                    ToNullableLong(dr["NullableLongNumber"]),
                    ToInt(dr["Integer"]),
                    ToNullableInt(dr["NullableInteger"]),
                    ToShort(dr["Short"]),
                    ToNullableShort(dr["NullableShort"]),
                    ToByte(dr["Byte"]),
                    ToNullableByte(dr["NullableByte"]),
                    ToDouble(dr["DoubleNumber"]),
                    ToNullableDouble(dr["NullableDoubleNumber"]),
                    ToDecimal(dr["DecimalNumber"]),
                    ToNullableDecimal(dr["NullableDecimalNumber"]),
                    ToString(dr["String"]),
                    ToChar(dr["Character"]),
                    ToNullableChar(dr["NullableCharacter"]),
                    ToDateTimeOffset(dr["Date"]),
                    ToNullableDateTimeOffset(dr["NullableDate"]),
                    ToBool(dr["Boolean"]),
                    ToNullableBool(dr["NullableBoolean"])
                    );
            }
        }

        public record EachPrimitiveType(
            long LongNumber,
            long? NullableLongNumber,
            int Integer,
            int? NullableInteger,
            short Short,
            short? NullableShort,
            byte Byte,
            byte? NullableByte,
            double DoubleNumber,
            double? NullableDoubleNumber,
            decimal DecimalNumber,
            decimal? NullableDecimalNumber,
            string String,
            char Character,
            char? NullableCharacter,
            DateTimeOffset Date,
            DateTimeOffset? NullableDate,
            bool Boolean,
            bool? NullableBoolean
            );
    }
}
