using BHS.Contracts;
using BHS.DataAccess.Tests;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class AuthorRepositoryTests
    {
        private readonly AuthorRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();

        public AuthorRepositoryTests()
        {
            Subject = new AuthorRepository(MockQuerier);
        }

        [Fact]
        public async Task GetByUserName_Executes()
        {
            MockQuerier.SingleResult = new Author(3, "s", "t");

            _ = await Subject.GetByUserName("b");

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("dbo.Author_GetByUserName", MockQuerier.CommandText);

            Assert.Equal("b", MockQuerier.Parameters.userName);
        }

        [Fact]
        public async Task GetAll_Executes()
        {
            MockQuerier.ManyResults = new Author[]
            {
                new Author(3, "s", "t")
            };

            _ = await Subject.GetAll();

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("dbo.Author_GetAll", MockQuerier.CommandText);
        }
    }
}
