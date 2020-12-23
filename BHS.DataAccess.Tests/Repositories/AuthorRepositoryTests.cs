using BHS.Contracts;
using BHS.DataAccess.Core;
using BHS.DataAccess.Tests;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class AuthorRepositoryTests
    {
        private readonly AuthorRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public AuthorRepositoryTests()
        {
            Subject = new AuthorRepository(new Querier(MockData.CreateDbConnectionFactory().Object));
        }

        [Fact]
        public async Task GetByUserName_FillsResult()
        {
            var author = new Author(3, "s", "t", true);
            MockData.SetReaderResultset(new Author[] { author });

            var result = await Subject.GetByUserName("a");

            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.DisplayName, result.DisplayName);
            Assert.Equal(author.Name, result.Name);
            Assert.Equal(author.IsVisible, result.IsVisible);
        }

        [Fact]
        public async Task GetByUserName_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetByUserName("b");

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("dbo.Author_GetByUserName", MockData.CommandText);

            Assert.Equal("@userName", MockData.Parameters[0].ParameterName);
            Assert.Equal("b", MockData.Parameters[0].Value);
        }

        [Fact]
        public async Task GetAll_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetAll().ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("dbo.Author_GetAll", MockData.CommandText);

            Assert.Equal("@doIncludeHidden", MockData.Parameters[0].ParameterName);
            Assert.Equal(false, MockData.Parameters[0].Value);
        }
    }
}
