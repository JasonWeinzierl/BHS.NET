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
            var post = new Post(1, default, default, default, default, default, default, default);
            MockData.ReaderResultset = ModelsFlattener.ToDataTable(new Post[] { post });

            var result = await Subject.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(post.Id, result.Id);
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.BodyContent, result.BodyContent);
            Assert.Equal(post.FilePath, result.FilePath);
            Assert.Equal(post.PhotosAlbumId, result.PhotosAlbumId);
            Assert.Equal(post.IsVisible, result.IsVisible);
            Assert.Equal(post.AuthorId, result.AuthorId);
            Assert.Equal(post.PublishDate, result.PublishDate);
        }

        [Fact]
        public async Task GetById_Bindings()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetById(1);

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.Post_GetById", MockData.CommandText);

            Assert.Equal("@id", MockData.Parameters[0].ParameterName);
            Assert.Equal(1, MockData.Parameters[0].Value);
        }
    }
}
