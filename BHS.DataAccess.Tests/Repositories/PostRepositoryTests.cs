using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostRepositoryTests
    {
        private readonly IPostRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        // todo: inject helper to writeline test messages
        public PostRepositoryTests()
        {
            var connectionFactory = MockData.CreateDbConnectionFactory();
            Subject = new PostRepository(connectionFactory.Object);
        }

        [Fact]
        public async Task GetById_FillsResult()
        {
            var post = new Post("a", default, default, default, default, default, default, default);
            MockData.ReaderResultset = ModelsFlattener.ToDataTable(new Post[] { post });

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
        public async Task GetById_Bindings()
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
