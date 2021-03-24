using BHS.DataAccess.Models;
using BHS.DataAccess.Tests;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class CategoryRepositoryTests
    {
        private readonly CategoryRepository _subject;

        private readonly MockExecuter _mockExecuter = new();

        public CategoryRepositoryTests()
        {
            _subject = new CategoryRepository(_mockExecuter);
        }

        [Fact]
        public async Task GetBySlug_Executes()
        {
            var category = new CategoryDTO("thing", "Thing!");
            var post = new PostPreviewDTO(string.Empty, string.Empty, string.Empty, default, default, default, default);
            _mockExecuter.TwoManyResults = (new[] { category }, new[] { post });

            _ = await _subject.GetBySlug("y");

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.Category_GetBySlug", _mockExecuter.CommandText);

            Assert.Equal("y", _mockExecuter.Parameters?.slug);
        }

        [Fact]
        public async Task GetBySlug_JoinsMultipleResults()
        {
            var category = new CategoryDTO("thing", "Thing!");
            var post1 = new PostPreviewDTO(string.Empty, string.Empty, string.Empty, default, default, default, default);
            var post2 = new PostPreviewDTO(string.Empty, string.Empty, string.Empty, default, default, default, default);
            _mockExecuter.TwoManyResults = (new[] { category }, new[] { post1, post2 });

            var result = await _subject.GetBySlug("y");

            Assert.NotNull(result);
            Assert.Equal(2, result?.Posts.Count());
        }

        [Fact]
        public async Task GetAll_Executes()
        {
            _mockExecuter.ManyResults = new CategorySummaryDTO[]
            {
                new CategorySummaryDTO("thing", "Thing!", 0)
            };

            _ = await _subject.GetAll();

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.CategorySummary_GetAll", _mockExecuter.CommandText);
        }
    }
}
