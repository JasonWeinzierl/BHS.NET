namespace BHS.Contracts;

public record ContactAlert(
    string Id,
    string? Name,
    string EmailAddress,
    string? Message,
    DateTimeOffset? DateRequested,
    DateTimeOffset DateCreated);
