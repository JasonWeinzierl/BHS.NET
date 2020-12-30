using BHS.DataAccess.Tests;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class QuerierTests
    {
        private readonly Querier Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public QuerierTests()
        {
            Subject = new Querier(MockData.CreateDbConnectionFactory().Object);
        }

        [Fact]
        public async Task ExecuteScalarAsync_PreparesCommand()
        {
            MockData.ScalarCell = new object();
            var mockFcn = new Mock<Action<IDbCommand>>();

            _ = await Subject.ExecuteScalarAsync<object>("connstr", "Get", mockFcn.Object);

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Get", MockData.CommandText);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            mockFcn.Verify(f => f.Invoke(It.IsAny<IDbCommand>()), Times.Once, "Expected configureCommand to be invoked");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2.0D)]
        [InlineData(100000000000L)]
        [InlineData('a')]
        [InlineData(true)]
        public async Task ExecuteScalarAsync_ReturnsPrimitive<T>(T inputValue)
        {
            MockData.ScalarCell = inputValue;

            var result = await Subject.ExecuteScalarAsync<T>("connstr", "Insert", null);

            Assert.Equal(inputValue, result);
        }

        [Fact]
        public async Task ExecuteReaderAsync_PreparesCommand()
        {
            MockData.ReaderResultset = new DataTable();
            var mockFcn = new Mock<Action<IDbCommand>>();

            _ = await Subject.ExecuteReaderAsync("connstr", "Get", mockFcn.Object, (IDataReader dr, ref object model) => { });

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Get", MockData.CommandText);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            mockFcn.Verify(f => f.Invoke(It.IsAny<IDbCommand>()), Times.Once, "Expected configureCommand to be invoked");
        }

        [Fact]
        public async Task ExecuteReaderAsync_ReadsEachRow()
        {
            var table = new DataTable();
            table.Columns.Add("Col1");
            table.Rows.Add("a");
            table.Rows.Add("b");
            table.Rows.Add("c");
            MockData.ReaderResultset = table;
            var mockFiller = new Mock<FillDelegate<object>>();

            _ = await Subject.ExecuteReaderAsync("connstr", "Get", null, mockFiller.Object);

            mockFiller.Verify(f => f(It.IsAny<IDataReader>(), ref It.Ref<object>.IsAny), Times.Exactly(3), "Expected fillOutput to be called for each row.");
        }

        [Fact]
        public async Task ExecuteReaderAsync_ReturnsEmptyObjectWhenNoRows()
        {
            MockData.ReaderResultset = new DataTable();

            var result = await Subject.ExecuteReaderAsync<List<int>>("connstr", "Get", null, (IDataReader dr, ref List<int> models) => { });

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteReaderAsync_AsyncEnumerable_PreparesCommand()
        {
            MockData.ReaderResultset = new DataTable();
            var mockFcn = new Mock<Action<IDbCommand>>();

            _ = await Subject.ExecuteReaderAsync("connstr", "Get", mockFcn.Object, (IDataRecord dr) => new object()).ToListAsync();

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Get", MockData.CommandText);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            mockFcn.Verify(f => f.Invoke(It.IsAny<IDbCommand>()), Times.Once, "Expected configureCommand to be invoked");
        }

        [Fact]
        public async Task ExecuteReaderAsync_AsyncEnumerable_ReadsAllData()
        {
            var table = new DataTable();
            table.Columns.Add("Col1");
            table.Rows.Add("val1");
            table.Rows.Add(DBNull.Value);
            table.Rows.Add("val2");
            MockData.ReaderResultset = table;

            var result = await Subject.ExecuteReaderAsync("connstr", "Get", null, dr => "Value").ToListAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal("Value", result[0]);
            Assert.Equal("Value", result[1]);
            Assert.Equal("Value", result[2]);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_PreparesCommand()
        {
            MockData.NonQueryRowsAffected = 1;
            var mockFcn = new Mock<Action<IDbCommand>>();

            _ = await Subject.ExecuteNonQueryAsync("connstr", "Save", mockFcn.Object);

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Save", MockData.CommandText);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            mockFcn.Verify(f => f.Invoke(It.IsAny<IDbCommand>()), Times.Once, "Expected configureCommand to be invoked");
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_ReturnsAffected()
        {
            MockData.NonQueryRowsAffected = 2;

            int affected = await Subject.ExecuteNonQueryAsync("connstr", "Save", null);

            Assert.Equal(2, affected);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_Parameters_PreparesCommand()
        {
            MockData.NonQueryRowsAffected = 1;
            var mockFcn = new Mock<Action<IDbCommand>>();

            _ = await Subject.ExecuteNonQueryAsync("connstr", "Get", mockFcn.Object, cmd => "Value");

            Assert.Equal("connstr", MockData.ConnectionStringName);
            Assert.Equal("Get", MockData.CommandText);
            Assert.Equal(CommandType.StoredProcedure, MockData.CommandType);
            mockFcn.Verify(f => f.Invoke(It.IsAny<IDbCommand>()), Times.Once, "Expected configureCommand to be invoked");
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_Parameters_ReadsFromCommand()
        {
            MockData.NonQueryRowsAffected = 1;
            var mockParamReader = new Mock<ParameterDelegate<object>>();
            mockParamReader.Setup(r => r(It.IsAny<IDbCommand>()))
                .Returns("Value");

            var result = await Subject.ExecuteNonQueryAsync("connstr", "Process", null, mockParamReader.Object);

            Assert.Equal("Value", result);
            mockParamReader.Verify();
        }
    }
}
