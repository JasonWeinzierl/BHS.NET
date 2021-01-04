using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System.Threading.Tasks;
using Xunit;
//using Xunit.Abstractions;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostRepositoryTests
    {
        private readonly PostRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();
        //private readonly ITestOutputHelper _output;

        public PostRepositoryTests(/*ITestOutputHelper output*/)
        {
            Subject = new PostRepository(MockQuerier);
            //_output = output;
        }

        [Fact]
        public async Task GetBySlug_Executes()
        {
            MockQuerier.SingleResult = new Post(default, default, default, default, default, default, default, default);

            _ = await Subject.GetBySlug("a");

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("blog.Post_GetBySlug", MockQuerier.CommandText);

            Assert.Equal("a", MockQuerier.Parameters.slug);
        }
    }
}
