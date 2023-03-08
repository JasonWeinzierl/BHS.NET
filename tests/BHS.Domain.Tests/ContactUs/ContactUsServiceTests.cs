using BHS.Contracts;
using BHS.Domain.ContactUs;
using BHS.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BHS.Domain.Tests.ContactUs;

public class ContactUsServiceTests
{
    private readonly ContactUsOptions _settings = new();
    private readonly Mock<IContactAlertRepository> _mockRepo = new(MockBehavior.Strict);
    private readonly Mock<IEmailAdapter> _mockEmailAdapter = new(MockBehavior.Strict);
    private readonly Mock<ILogger<ContactUsService>> _mockLogger = new();

    private ContactUsService Subject => new(Options.Create(_settings), _mockRepo.Object, _mockEmailAdapter.Object, _mockLogger.Object);

    private void VerifyAll() => Mock.VerifyAll(_mockRepo, _mockEmailAdapter, _mockLogger);

    public class AddRequest : ContactUsServiceTests
    {
        [Fact]
        public async Task OnBodyEmpty_InsertsAndSendsMessage()
        {
            // Arrange
            var request = new ContactAlertRequest(default, "x", default, default, null);
            _mockRepo
                .Setup(r => r.Insert(request, default))
                .ReturnsAsync(() => new ContactAlert("1", default, string.Empty, default, default, default));
            _mockEmailAdapter
                .Setup(c => c.Send(It.IsAny<EmailMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new EmailMessageResponse(System.Net.HttpStatusCode.OK, new StringContent("")));

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
            _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>(), default), Times.Never, "Expected insert to never be called if body (honeypot) has value.");
            _mockEmailAdapter.Verify(c => c.Send(It.IsAny<EmailMessageRequest>(), It.IsAny<CancellationToken>()), Times.Never, "Expected message to never be sent if body (honeypot) has value.");
        }

        [Fact]
        public async Task OnEmailEmpty_Throws()
        {
            // Arrange
            var request = new ContactAlertRequest(default, string.Empty, default, default, null);

            await Assert.ThrowsAsync<InvalidContactRequestException>(async () =>
            {
                // Act
                _ = await Subject.AddRequest(request);
            });

            // Assert
            _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>(), default), Times.Never, "Expected insert to never be called if email is empty.");
            _mockEmailAdapter.Verify(c => c.Send(It.IsAny<EmailMessageRequest>(), It.IsAny<CancellationToken>()), Times.Never, "Expected message to never be sent if email is empty.");
        }
    }
}
