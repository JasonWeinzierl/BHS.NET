using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class CategoryRepositoryTests
    {
        private readonly CategoryRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public CategoryRepositoryTests()
        {
            Subject = new CategoryRepository(MockData.CreateDbConnectionFactory().Object);
        }

        [Fact]
        public async Task GetBySlug_FillsResult()
        {
            var category = new Category("thing", "Thing!");
            MockData.SetReaderResultset(new Category[] { category });

            var result = await Subject.GetBySlug("x");

            Assert.NotNull(result);
            Assert.Equal(category.Slug, result.Slug);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async Task GetBySlug_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetBySlug("y");

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.Category_GetBySlug", MockData.CommandText);

            Assert.Equal("@slug", MockData.Parameters[0].ParameterName);
            Assert.Equal("y", MockData.Parameters[0].Value);
        }

        [Fact]
        public async Task GetByPostSlug_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetByPostSlug("z").ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.Category_GetByPostSlug", MockData.CommandText);

            Assert.Equal("@postSlug", MockData.Parameters[0].ParameterName);
            Assert.Equal("z", MockData.Parameters[0].Value);
        }
    }
}
