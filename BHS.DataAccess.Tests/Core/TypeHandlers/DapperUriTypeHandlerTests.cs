using Moq;
using System;
using System.Data;
using Xunit;

namespace BHS.DataAccess.Core.TypeHandlers.Tests
{
    public class DapperUriTypeHandlerTests
    {
        private readonly DapperUriTypeHandler _subject;

        public DapperUriTypeHandlerTests()
        {
            _subject = new DapperUriTypeHandler();
        }

        [Fact]
        public void Format_ConvertsToString()
        {
            var uri = new Uri("scheme:path");

            object? result = null;
            var mockParameter = new Mock<IDbDataParameter>();
            mockParameter.SetupSet(p => p.Value = It.IsAny<string>())
                .Callback<object?>(s => result = s);

            _subject.SetValue(mockParameter.Object, uri);

            Assert.Equal("scheme:path", result);
        }

        [Fact]
        public void Parse_ConvertsToUri()
        {
            string cell = "scheme:path";

            var result = _subject.Parse(cell);

            Assert.Equal(new Uri("scheme:path", UriKind.RelativeOrAbsolute), result);
        }
    }
}
