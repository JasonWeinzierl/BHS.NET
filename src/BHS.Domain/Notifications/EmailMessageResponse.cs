using System.Net;

namespace BHS.Domain.Notifications;

public record EmailMessageResponse(
    HttpStatusCode StatusCode,
    HttpContent Body)
{
    public bool IsSuccessStatusCode => 200 <= (int)StatusCode && (int)StatusCode <= 299;
}
