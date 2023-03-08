namespace BHS.Domain.Notifications;

public record EmailMessageRequest(
    IEnumerable<string> ToAddresses,
    string Subject,
    string MessageHtml,
    string MessagePlainText);
