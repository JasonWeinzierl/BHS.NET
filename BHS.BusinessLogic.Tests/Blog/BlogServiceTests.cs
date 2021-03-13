using BHS.Contracts;
using BHS.Model.DataAccess;
using BHS.Model.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
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
            _mockPostRepo = new Mock<IPostRepository>();
            _mockPreviewRepo = new Mock<IPostPreviewRepository>();
            _mockCatRepo = new Mock<ICategoryRepository>();
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _logger = new Mock<ILogger<BlogService>>();

            _subject = new BlogService(_mockPostRepo.Object, _mockPreviewRepo.Object, _mockCatRepo.Object, _mockAuthorRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetCategory_CallsGetBySlug()
        {
            string slug = "a";

            _ = await _subject.GetCategory(slug);

            _mockCatRepo.Verify(r => r.GetBySlug(It.Is<string>(s => s == slug)));
        }

        [Fact]
        public async Task GetPostsByCategory_CallsGetByPostSlug()
        {
            string slug = "b";

            _ = await _subject.GetPostsByCategory(slug);

            _mockPreviewRepo.Verify(r => r.GetByCategorySlug(It.Is<string>(s => s == slug)));
        }

        [Fact]
        public async Task GetPost_CallsGetBySlug()
        {
            string slug = "c";

            _ = await _subject.GetPost(slug);

            _mockPostRepo.Verify(r => r.GetBySlug(It.Is<string>(s => s == slug)));
        }

        [Fact]
        public async Task GetCategoriesByPost_CallsGetBySlug()
        {
            string slug = "d";

            _ = await _subject.GetCategoriesByPost(slug);

            _mockCatRepo.Verify(r => r.GetByPostSlug(It.Is<string>(s => s == slug)));
        }

        [Fact]
        public async Task GetPostsByAuthor_OnAuthorFound_CallsGetByAuthorId()
        {
            string uname = "a";
            int id = 1;
            _mockAuthorRepo.Setup(r => r.GetByUserName(It.IsAny<string>()))
                .ReturnsAsync(new Author(id, "some user", "User"));

            _ = await _subject.GetPostsByAuthor(uname);

            _mockAuthorRepo.Verify(r => r.GetByUserName(It.Is<string>(s => s == uname)), Times.Once, "Expected GetByUserName to be called once.");
            _mockPreviewRepo.Verify(r => r.GetByAuthorId(It.Is<int>(i => i == id)));
        }

        [Fact]
        public async Task GetPostsByAuthor_OnNoPostsFound_ReturnsEmpty()
        {
            string uname = "a";
            int id = 3;
            _mockAuthorRepo.Setup(r => r.GetByUserName(It.IsAny<string>()))
                .ReturnsAsync(new Author(id, "a", default));

            var result = await _subject.GetPostsByAuthor(uname);

            Assert.NotNull(result);
            _mockAuthorRepo.Verify(r => r.GetByUserName(It.Is<string>(s => s == uname)), Times.Once, "Expected GetByUserName to be called once.");
            _mockPreviewRepo.Verify(r => r.GetByAuthorId(It.Is<int>(i => i == id)));
        }

        [Fact]
        public async Task GetPostsByAuthor_OnAuthorNotFound_Throws()
        {
            string uname = "a";

            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                _ = await _subject.GetPostsByAuthor(uname);
            });

            _mockAuthorRepo.Verify(r => r.GetByUserName(It.Is<string>(s => s == uname)), Times.Once, "Expected GetByUserName to be called once.");
            _mockPreviewRepo.Verify(r => r.GetByAuthorId(It.IsAny<int>()), Times.Never, "Expected GetByAuthorId to never be called.");
        }

        [Fact]
        public async Task SearchPosts_CallsSearch()
        {
            string text = "abc";
            var from = new DateTime(2021, 01, 06, 0, 0, 0);
            var to = new DateTime(2021, 01, 06, 0, 1, 0);

            _ = await _subject.SearchPosts(text, from, to);

            _mockPreviewRepo.Verify(r => r.Search(It.Is<string>(s => s == text),
                It.Is<DateTimeOffset>(f => f == from),
                It.Is<DateTimeOffset>(t => t == to)));
        }
    }
}
