namespace BHS.Contracts
{
    public record ContactAlertRequest(
        string? Name,
        string EmailAddress,
        string? Message,
        DateTimeOffset? DateRequested,
        string? Body);
}
