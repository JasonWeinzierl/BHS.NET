using BHS.Contracts;
using BHS.Domain;
using BHS.Domain.Exceptions;
using BHS.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using Xunit;

namespace BHS.BusinessLogic.Tests
{
    public class ContactUsServiceTests
    {
        private readonly ContactUsOptions _settings = new();
        private readonly Mock<IContactAlertRepository> _mockRepo = new(MockBehavior.Strict);
        private readonly Mock<ISendGridClient> _mockSgClient = new(MockBehavior.Strict);
        private readonly Mock<ILogger<ContactUsService>> _mockLogger = new();
        
        private ContactUsService Subject => new(Options.Create(_settings), _mockRepo.Object, _mockSgClient.Object, _mockLogger.Object);

        private void VerifyAll() => Mock.VerifyAll(_mockRepo, _mockSgClient, _mockLogger);

        public class AddRequest : ContactUsServiceTests
        {
            [Fact]
            public async Task OnBodyEmpty_InsertsAndSendsMessage()
            {
                // Arrange
                var request = new ContactAlertRequest(default, "x", default, default, null);
                _mockRepo
                    .Setup(r => r.Insert(request))
                    .ReturnsAsync(() => new ContactAlert(default, default, string.Empty, default, default, default));
                _mockSgClient
                    .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => new Response(System.Net.HttpStatusCode.OK, default, default));

                // Act
                var result = await Subject.AddRequest(request);

                // Assert
                Assert.NotNull(result);
                VerifyAll();
            }

            [Fact]
            public async Task OnBodyHasValue_DoesNothing()
            {
                // Arrange
                var request = new ContactAlertRequest(default, string.Empty, default, default, "something");

                // Act
                var result = await Subject.AddRequest(request);

                // Assert
                Assert.Null(result);
                _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>()), Times.Never, "Expected insert to never be called if body (honeypot) has value.");
                _mockSgClient.Verify(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never, "Expected message to never be sent if body (honeypot) has value.");
            }

            [Fact]
            public async Task OnEmailEmpty_Throws()
            {
                // Arrange
                var request = new ContactAlertRequest(default, string.Empty, default, default, null);

                await Assert.ThrowsAsync<BadRequestException>(async () =>
                {
                    // Act
                    _ = await Subject.AddRequest(request);
                });

                // Assert
                _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>()), Times.Never, "Expected insert to never be called if email is empty.");
                _mockSgClient.Verify(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never, "Expected message to never be sent if email is empty.");
            }
        }
    }
}
