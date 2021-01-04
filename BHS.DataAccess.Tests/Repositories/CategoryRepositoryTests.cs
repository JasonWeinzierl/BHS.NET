using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class CategoryRepositoryTests
    {
        private readonly CategoryRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();

        public CategoryRepositoryTests()
        {
            Subject = new CategoryRepository(MockQuerier);
        }

        [Fact]
        public async Task GetBySlug_Executes()
        {
            MockQuerier.SingleResult = new Category("thing", "Thing!");

            _ = await Subject.GetBySlug("y");

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("blog.Category_GetBySlug", MockQuerier.CommandText);

            Assert.Equal("y", MockQuerier.Parameters.slug);
        }

        [Fact]
        public async Task GetByPostSlug_Executes()
        {
            MockQuerier.ManyResults = new Category[]
            {
                new Category("thing", "Thing!")
            };

            _ = await Subject.GetByPostSlug("z");

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("blog.Category_GetByPostSlug", MockQuerier.CommandText);

            Assert.Equal("z", MockQuerier.Parameters.postSlug);
        }
    }
}
