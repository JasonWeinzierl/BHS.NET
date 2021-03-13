using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System.Threading.Tasks;
using Xunit;
//using Xunit.Abstractions;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostRepositoryTests
    {
        private readonly PostRepository _subject;

        private readonly MockExecuter _mockExecuter = new();
        //private readonly ITestOutputHelper _output;

        public PostRepositoryTests(/*ITestOutputHelper output*/)
        {
            _subject = new PostRepository(_mockExecuter);
            //_output = output;
        }

        [Fact]
        public async Task GetBySlug_Executes()
        {
            _mockExecuter.SingleResult = new Post(string.Empty, string.Empty, string.Empty, default, default, default, default, default);

            _ = await _subject.GetBySlug("a");

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.Post_GetBySlug", _mockExecuter.CommandText);

            Assert.Equal("a", _mockExecuter.Parameters?.slug);
        }
    }
}
