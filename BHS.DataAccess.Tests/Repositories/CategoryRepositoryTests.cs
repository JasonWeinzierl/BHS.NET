using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
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
            _mockExecuter.SingleResult = new Category("thing", "Thing!");

            _ = await _subject.GetBySlug("y");

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.Category_GetBySlug", _mockExecuter.CommandText);

            Assert.Equal("y", _mockExecuter.Parameters?.slug);
        }

        [Fact]
        public async Task GetByPostSlug_Executes()
        {
            _mockExecuter.ManyResults = new Category[]
            {
                new Category("thing", "Thing!")
            };

            _ = await _subject.GetByPostSlug("z");

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.Category_GetByPostSlug", _mockExecuter.CommandText);

            Assert.Equal("z", _mockExecuter.Parameters?.postSlug);
        }
    }
}
