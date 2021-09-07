using BHS.Contracts;
using BHS.Contracts.Blog;
using BHS.Domain.Repositories;
using BHS.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BHS.BusinessLogic.Blog.Tests
{
    public class BlogServiceTests
    {
        private readonly BlogService _subject;

        private readonly Mock<IPostRepository> _mockPostRepo;
        private readonly Mock<IPostPreviewRepository> _mockPreviewRepo;
        private readonly Mock<ICategoryRepository> _mockCatRepo;
        private readonly Mock<IAuthorRepository> _mockAuthorRepo;
        private readonly Mock<ILogger<BlogService>> _logger;

        public BlogServiceTests()
        {
            _mockPostRepo = new Mock<IPostRepository>(MockBehavior.Strict);
            _mockPreviewRepo = new Mock<IPostPreviewRepository>(MockBehavior.Strict);
            _mockCatRepo = new Mock<ICategoryRepository>(MockBehavior.Strict);
            _mockAuthorRepo = new Mock<IAuthorRepository>(MockBehavior.Strict);
            _logger = new Mock<ILogger<BlogService>>();

            _subject = new BlogService(_mockPostRepo.Object, _mockPreviewRepo.Object, _mockCatRepo.Object, _mockAuthorRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetCategory_IfExists_GetsPosts()
        {
            string slug = "a";
            _mockCatRepo.Setup(r => r.GetBySlug(slug))
                .ReturnsAsync(new Category("x", "y"));
            _mockPreviewRepo.Setup(r => r.GetByCategorySlug(slug))
                .ReturnsAsync(Array.Empty<PostPreview>());

            var result = await _subject.GetCategory(slug);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetCategory_IfNotExists_ReturnsNull()
        {
            string slug = "b";
            _mockCatRepo.Setup(r => r.GetBySlug(slug))
                .ReturnsAsync((Category?)null);

            var result = await _subject.GetCategory(slug);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPost_CallsGetBySlug()
        {
            string slug = "c";
            var expected = new Post(slug, string.Empty, string.Empty, null, null, null, default, default, Array.Empty<Category>());
            _mockPostRepo.Setup(r => r.GetBySlug(slug))
                .ReturnsAsync(expected);

            var result = await _subject.GetPost(slug);

            Assert.Equal(result, expected);
        }

        [Fact]
        public async Task GetCategories_CallsGetAll()
        {
            var expected = Array.Empty<CategorySummary>();
            _mockCatRepo.Setup(r => r.GetAll()).ReturnsAsync(expected);

            var result = await _subject.GetCategories();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetPostsByAuthor_OnAuthorFound_CallsGetByAuthorId()
        {
            string uname = "a";
            int id = 1;
            var expected = Array.Empty<PostPreview>();
            _mockAuthorRepo.Setup(r => r.GetByUserName(uname))
                .ReturnsAsync(new Author(id, "some user", "User"));
            _mockPreviewRepo.Setup(r => r.GetByAuthorId(id))
                .ReturnsAsync(expected);

            var result = await _subject.GetPostsByAuthor(uname);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetPostsByAuthor_OnNoPostsFound_ReturnsEmpty()
        {
            string uname = "a";
            int id = 3;
            _mockAuthorRepo.Setup(r => r.GetByUserName(It.IsAny<string>()))
                .ReturnsAsync(new Author(id, "a", default));
            _mockPreviewRepo.Setup(r => r.GetByAuthorId(id))
                .ReturnsAsync(Array.Empty<PostPreview>());

            var result = await _subject.GetPostsByAuthor(uname);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetPostsByAuthor_OnAuthorNotFound_Throws()
        {
            string uname = "a";
            _mockAuthorRepo.Setup(r => r.GetByUserName(uname))
                .ReturnsAsync((Author?)null);

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await _subject.GetPostsByAuthor(uname);
            });

            _mockPreviewRepo.Verify(r => r.GetByAuthorId(It.IsAny<int>()), Times.Never, "Expected GetByAuthorId to never be called.");
        }

        [Fact]
        public async Task SearchPosts_CallsSearch()
        {
            string text = "abc";
            var from = new DateTime(2021, 01, 06, 0, 0, 0);
            var to = new DateTime(2021, 01, 06, 0, 1, 0);
            _mockPreviewRepo.Setup(r => r.Search(text, from, to))
                .ReturnsAsync(Array.Empty<PostPreview>());

            var result = await _subject.SearchPosts(text, from, to);

            Assert.Empty(result);
        }
    }
}
