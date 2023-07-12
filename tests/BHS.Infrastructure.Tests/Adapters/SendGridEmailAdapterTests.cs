using BHS.Domain.Notifications;
using BHS.Infrastructure.Adapters;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using Xunit;

namespace BHS.Infrastructure.Tests.Adapters;

public class SendGridEmailAdapterTests
{
    private readonly NotificationOptions _options = new();
    private readonly Mock<ISendGridClient> _mockSgClient = new(MockBehavior.Strict);

    private SendGridEmailAdapter Subject => new(Options.Create(_options), _mockSgClient.Object);

    [Fact]
    public async Task SendsMessage()
    {
        // Arrange
        _options.FromAddress = "test@test.com";
        _options.FromName = "test person";
        var request = new EmailMessageRequest(new[] { "test2@test.com" }, "test subject", "<p>Hi</p>", "Hi");

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

        SendGridMessage? sent = null;
        _mockSgClient
            .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Response(httpResponse.StatusCode, httpResponse.Content, httpResponse.Headers))
            .Callback((SendGridMessage m, CancellationToken ct) => sent = m);

        // Act
        var response = await Subject.Send(request);

        // Assert
        _mockSgClient.Verify(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), default), Times.Once);
        _mockSgClient.VerifyNoOtherCalls();

        Assert.NotNull(sent);
        Assert.Equal("test@test.com", sent.From.Email);
        Assert.Equal("test person", sent.From.Name);
        Assert.Single(sent.Personalizations);
        Assert.Single(sent.Personalizations[0].Tos);
        Assert.Equal("test2@test.com", sent.Personalizations[0].Tos[0].Email);
        Assert.Equal("test subject", sent.Subject);
        Assert.Equal("<p>Hi</p>", sent.HtmlContent);
        Assert.Equal("Hi", sent.PlainTextContent);
    }

    [Fact]
    public async Task IfNoToAddresses_Throws()
    {
        var request = new EmailMessageRequest(Array.Empty<string>(), "", "", "");

        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            _ = await Subject.Send(request);
        });

        Assert.Equal("request", ex.ParamName);
    }
}
