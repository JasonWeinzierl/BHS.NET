using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.DataAccess.Tests;
using System.Data;
using System.Threading.Tasks;
using Xunit;
//using Xunit.Abstractions;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostRepositoryTests
    {
        private readonly PostRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();
        //private readonly ITestOutputHelper _output;

        public PostRepositoryTests(/*ITestOutputHelper output*/)
        {
            Subject = new PostRepository(new Querier(MockData.CreateDbConnectionFactory().Object));
            //_output = output;
        }

        [Fact]
        public async Task GetBySlug_FillsResult()
        {
            var post = new Post("a", default, default, default, default, default, default, default);
            MockData.SetReaderResultset(new Post[] { post });

            var result = await Subject.GetBySlug("a");

            Assert.NotNull(result);
            Assert.Equal(post.Slug, result.Slug);
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.ContentMarkdown, result.ContentMarkdown);
            Assert.Equal(post.FilePath, result.FilePath);
            Assert.Equal(post.PhotosAlbumId, result.PhotosAlbumId);
            Assert.Equal(post.AuthorId, result.AuthorId);
            Assert.Equal(post.DatePublished, result.DatePublished);
            Assert.Equal(post.DateLastModified, result.DateLastModified);
        }

        [Fact]
        public async Task GetBySlug_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetBySlug("a");

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.Post_GetBySlug", MockData.CommandText);

            Assert.Equal("@slug", MockData.Parameters[0].ParameterName);
            Assert.Equal("a", MockData.Parameters[0].Value);
        }
    }
}
