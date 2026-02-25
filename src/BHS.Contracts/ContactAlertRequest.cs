namespace BHS.Contracts;

public record ContactAlertRequest(
    string? Name,
    string EmailAddress,
    string? Message = null,
    DateTimeOffset? DateRequested = null,
    string? Body = null);
