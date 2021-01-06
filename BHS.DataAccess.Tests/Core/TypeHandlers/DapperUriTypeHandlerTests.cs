using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Core.TypeHandlers.Tests
{
    public class DapperUriTypeHandlerTests
    {
        private readonly DapperUriTypeHandler Subject;

        public DapperUriTypeHandlerTests()
        {
            Subject = new DapperUriTypeHandler();
        }

        [Fact]
        public void Format_ConvertsToString()
        {
            var uri = new Uri("scheme:path");

            object result = null;
            var mockParameter = new Mock<IDbDataParameter>();
            mockParameter.SetupSet(p => p.Value = It.IsAny<string>())
                .Callback<object>(s => result = s);

            Subject.SetValue(mockParameter.Object, uri);

            Assert.Equal("scheme:path", result);
        }

        [Fact]
        public void Parse_ConvertsToUri()
        {
            string cell = "scheme:path";

            var result = Subject.Parse(cell);

            Assert.Equal(new Uri("scheme:path", UriKind.RelativeOrAbsolute), result);
        }
    }
}
