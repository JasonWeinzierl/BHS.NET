namespace BHS.Domain.Notifications;

public interface IEmailAdapter
{
    Task<EmailMessageResponse> Send(EmailMessageRequest emailMessage, CancellationToken cancellationToken = default);
}
