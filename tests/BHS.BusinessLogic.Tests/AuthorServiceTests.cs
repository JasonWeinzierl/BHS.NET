using BHS.Contracts;
using BHS.Domain.DataAccess;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
            _mockRepo = new Mock<IAuthorRepository>(MockBehavior.Strict);
            _logger = new Mock<ILogger<AuthorService>>();
            Subject = new AuthorService(_mockRepo.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAuthor_CallsGetByUserName()
        {
            string username = "bob";
            _mockRepo.Setup(r => r.GetByUserName(username))
                .ReturnsAsync((Author?)null);

            var result = await Subject.GetAuthor(username);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAuthors_CallsGetAll()
        {
            _mockRepo.Setup(r => r.GetAll())
                .ReturnsAsync(Array.Empty<Author>());

            var result = await Subject.GetAuthors();

            Assert.Empty(result);
        }
    }
}
