using BHS.Domain.Notifications;
using BHS.Infrastructure.Adapters;
using Microsoft.Extensions.Options;
using NSubstitute;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using Xunit;

namespace BHS.Infrastructure.Tests.Adapters;

public class SendGridEmailAdapterTests
{
    private readonly NotificationOptions _options = new();
    private readonly ISendGridClient _sgClient = Substitute.For<ISendGridClient>();

    private SendGridEmailAdapter Subject => new(Options.Create(_options), _sgClient);

    [Fact]
    public async Task SendsMessage()
    {
        // Arrange
        _options.FromAddress = "test@test.com";
        _options.FromName = "test person";
        var request = new EmailMessageRequest(["test2@test.com"], "test subject", "<p>Hi</p>", "Hi");

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

        SendGridMessage? sent = null;
        _sgClient
            .SendEmailAsync(Arg.Do<SendGridMessage>(m => sent = m), Arg.Any<CancellationToken>())
            .Returns(new Response(httpResponse.StatusCode, httpResponse.Content, httpResponse.Headers));

        // Act
        var response = await Subject.Send(request, TestContext.Current.CancellationToken);

        // Assert
        await _sgClient.Received(1).SendEmailAsync(Arg.Any<SendGridMessage>(), Arg.Any<CancellationToken>());

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
        var request = new EmailMessageRequest([], "", "", "");

        var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            _ = await Subject.Send(request, TestContext.Current.CancellationToken);
        });

        Assert.Equal("request", ex.ParamName);
    }
}
