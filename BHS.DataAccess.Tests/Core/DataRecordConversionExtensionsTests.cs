using Moq;
using System;
using System.Data;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class DataRecordConversionExtensionsTests
    {
        private readonly Mock<IDataRecord> MockRecord;
        private object ColumnValue;

        public DataRecordConversionExtensionsTests()
        {
            MockRecord = new Mock<IDataRecord>();
            MockRecord.Setup(r => r[It.IsAny<string>()])
                .Returns(() => ColumnValue);
        }

        [Fact]
        public void CastLong_FromLong()
        {
            ColumnValue = 12345L;

            var result = MockRecord.Object.CastLong("a");

            Assert.Equal(12345L, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastLong_FromString()
        {
            ColumnValue = "12345";

            var result = MockRecord.Object.CastLong("b");

            Assert.Equal(12345L, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableLong_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableLong("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastInt_FromInt()
        {
            ColumnValue = 12345;

            var result = MockRecord.Object.CastInt("a");

            Assert.Equal(12345, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastInt_FromString()
        {
            ColumnValue = "12345";

            var result = MockRecord.Object.CastInt("b");

            Assert.Equal(12345, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableInt_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableInt("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastShort_FromShort()
        {
            ColumnValue = 1234;

            var result = MockRecord.Object.CastShort("a");

            Assert.Equal(1234, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastShort_FromString()
        {
            ColumnValue = "1234";

            var result = MockRecord.Object.CastShort("b");

            Assert.Equal(1234, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableShort_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableShort("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastByte_FromByte()
        {
            ColumnValue = 123;

            var result = MockRecord.Object.CastByte("a");

            Assert.Equal(123, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastByte_FromString()
        {
            ColumnValue = "123";

            var result = MockRecord.Object.CastByte("b");

            Assert.Equal(123, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableByte_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableByte("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastDouble_FromDouble()
        {
            ColumnValue = 1.23;

            var result = MockRecord.Object.CastDouble("a");

            Assert.Equal(1.23, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastDouble_FromString()
        {
            ColumnValue = "1.23";

            var result = MockRecord.Object.CastDouble("b");

            Assert.Equal(1.23, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastDouble_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastDouble("c");

            Assert.True(double.IsNaN(result));
            MockRecord.Verify(r => r["c"], Times.Once);
        }

        [Fact]
        public void CastNullableDouble_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableDouble("d");

            Assert.Null(result);
            MockRecord.Verify(r => r["d"], Times.Once);
        }


        [Fact]
        public void CastDecimal_FromDecimal()
        {
            ColumnValue = 1.23M;

            var result = MockRecord.Object.CastDecimal("a");

            Assert.Equal(1.23M, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastDecimal_FromString()
        {
            ColumnValue = "1.23";

            var result = MockRecord.Object.CastDecimal("b");

            Assert.Equal(1.23M, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableDecimal_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableDecimal("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastString_Happy()
        {
            ColumnValue = 1.23M;

            var result = MockRecord.Object.CastString("a");

            Assert.Equal("1.23", result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }


        [Fact]
        public void CastChar_FromChar()
        {
            ColumnValue = 'a';

            var result = MockRecord.Object.CastChar("a");

            Assert.Equal('a', result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastChar_FromString()
        {
            ColumnValue = "a";

            var result = MockRecord.Object.CastChar("b");

            Assert.Equal('a', result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableChar_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableChar("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastDateTimeOffset_FromDateTimeOffset()
        {
            var date = new DateTimeOffset(2020, 12, 22, 22, 06, 00, TimeSpan.FromHours(-6));
            ColumnValue = date;

            var result = MockRecord.Object.CastDateTimeOffset("a");

            Assert.Equal(date, result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastDateTimeOffset_FromString()
        {
            ColumnValue = "2020-12-22T22:07:00-06:00";
            var expectedDate = new DateTimeOffset(2020, 12, 22, 22, 07, 00, TimeSpan.FromHours(-6));

            var result = MockRecord.Object.CastDateTimeOffset("b");

            Assert.Equal(expectedDate, result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastNullableDateTimeOffset_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableDateTimeOffset("c");

            Assert.Null(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }


        [Fact]
        public void CastBool_FromBoolean()
        {
            ColumnValue = true;

            var result = MockRecord.Object.CastBool("a");

            Assert.True(result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }

        [Fact]
        public void CastBool_FromString()
        {
            ColumnValue = "true";

            var result = MockRecord.Object.CastBool("b");

            Assert.True(result);
            MockRecord.Verify(r => r["b"], Times.Once);
        }

        [Fact]
        public void CastBool_FromInt()
        {
            ColumnValue = 1;

            var result = MockRecord.Object.CastBool("c");

            Assert.True(result);
            MockRecord.Verify(r => r["c"], Times.Once);
        }

        [Fact]
        public void CastBool_ThrowsOnInvalidInt()
        {
            ColumnValue = 3;

            Assert.Throws<FormatException>(() => MockRecord.Object.CastBool("d"));
        }

        [Fact]
        public void CastNullableBool_FromDBNull()
        {
            ColumnValue = DBNull.Value;

            var result = MockRecord.Object.CastNullableBool("e");

            Assert.Null(result);
            MockRecord.Verify(r => r["e"], Times.Once);
        }


        [Fact]
        public void CastUri_FromString()
        {
            ColumnValue = "scheme:path";

            var result = MockRecord.Object.CastUri("a", UriKind.Absolute);

            Assert.Equal(new Uri("scheme:path", UriKind.Absolute), result);
            MockRecord.Verify(r => r["a"], Times.Once);
        }
    }
}
