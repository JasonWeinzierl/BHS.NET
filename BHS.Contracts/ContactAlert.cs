using System;

namespace BHS.Contracts
{
    public record ContactAlert(
        int Id,
        string Name,
        string EmailAddress,
        string Message,
        DateTimeOffset? DateRequested,
        DateTimeOffset DateCreated);
}
