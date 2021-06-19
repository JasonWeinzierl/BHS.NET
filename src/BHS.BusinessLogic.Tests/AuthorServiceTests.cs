using BHS.Model.DataAccess;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.BusinessLogic.Tests
{
    public class AuthorServiceTests
    {
        private readonly AuthorService Subject;

        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly Mock<ILogger<AuthorService>> _logger;

        public AuthorServiceTests()
        {
            _mockRepo = new Mock<IAuthorRepository>();
            _logger = new Mock<ILogger<AuthorService>>();
            Subject = new AuthorService(_mockRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAuthor_CallsGetByUserName()
        {
            string username = "bob";

            _ = await Subject.GetAuthor(username);

            _mockRepo.Verify(r => r.GetByUserName(It.Is<string>(u => u == username)));
        }

        [Fact]
        public async Task GetAuthors_CallsGetAll()
        {
            _ = await Subject.GetAuthors();

            _mockRepo.Verify(r => r.GetAll());
        }
    }
}
