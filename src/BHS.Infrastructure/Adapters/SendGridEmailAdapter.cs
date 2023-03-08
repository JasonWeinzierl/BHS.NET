using BHS.Domain.Notifications;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BHS.Infrastructure.Adapters;

public class SendGridEmailAdapter : IEmailAdapter
{
    private readonly NotificationOptions _options;
    private readonly ISendGridClient _sendGridClient;

    public SendGridEmailAdapter(
        IOptions<NotificationOptions> options,
        ISendGridClient sendGridClient)
    {
        _options = options.Value;
        _sendGridClient = sendGridClient;
    }

    public async Task<EmailMessageResponse> Send(EmailMessageRequest request, CancellationToken cancellationToken = default)
    {
        var toAddresses = request.ToAddresses.Select(to => new EmailAddress(to)).ToList();
        if (!toAddresses.Any())
            throw new ArgumentException("Request ToAddresses cannot be empty.", nameof(request));

        var sendGridMessage = new SendGridMessage
        {
            From = new EmailAddress(_options.FromAddress, _options.FromName),
            Subject = request.Subject,
            HtmlContent = request.MessageHtml,
            PlainTextContent = request.MessagePlainText,
        };
        sendGridMessage.AddTos(toAddresses);

        var response = await _sendGridClient.SendEmailAsync(sendGridMessage, cancellationToken);

        return new EmailMessageResponse(response.StatusCode, response.Body);
    }
}
