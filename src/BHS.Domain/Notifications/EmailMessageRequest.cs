namespace BHS.Domain.Notifications;

public record EmailMessageRequest(
    string FromAddress,
    string FromName,
    IEnumerable<string> ToAddresses,
    string Subject,
    string MessageHtml,
    string MessagePlainText);
