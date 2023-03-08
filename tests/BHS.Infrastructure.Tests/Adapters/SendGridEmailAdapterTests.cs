using BHS.Domain.Notifications;
using BHS.Infrastructure.Adapters;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using Xunit;

namespace BHS.Infrastructure.Tests.Adapters;

public class SendGridEmailAdapterTests
{
    private readonly Mock<ISendGridClient> _mockSgClient = new(MockBehavior.Strict);

    private SendGridEmailAdapter Subject => new(_mockSgClient.Object);

    [Fact]
    public async Task SendsMessage()
    {
        // Arrange
        var request = new EmailMessageRequest("test@test.com", "test person", new[] { "test@test.com" }, "test subject", "<p>Hi</p>", "Hi");

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
        _mockSgClient
            .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(httpResponse.StatusCode, httpResponse.Content, httpResponse.Headers));

        // Act
        var response = await Subject.Send(request);

        // Assert
        _mockSgClient.Verify(c => c.SendEmailAsync(It.Is<SendGridMessage>(msg => msg.Subject == "test subject"), default), Times.Once);
        _mockSgClient.VerifyNoOtherCalls();
    }
}
