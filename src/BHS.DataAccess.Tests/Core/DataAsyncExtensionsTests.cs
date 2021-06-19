using Moq;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Core.Tests
{
    public class DataAsyncExtensionsTests
    {
        [Fact]
        public async Task OpenAsync_OnDbConnection_CallsOpenAsync()
        {
            var mockDbConnection = new Mock<DbConnection>();

            await DataAsyncExtensions.OpenAsync(mockDbConnection.Object);

            mockDbConnection.Verify(c => c.OpenAsync(It.Is<CancellationToken>(t => t == CancellationToken.None)), Times.Once, "Expected OpenAsync to be called.");
        }

        [Fact]
        public async Task OpenAsync_OnNotDbConnection_RunsOpenInTask()
        {
            var mockDbConnection = new Mock<IDbConnection>();

            await DataAsyncExtensions.OpenAsync(mockDbConnection.Object);

            mockDbConnection.Verify(c => c.Open(), Times.Once, "Expected Open to be called.");
        }
    }
}
