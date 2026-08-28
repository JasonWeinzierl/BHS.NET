namespace BHS.Contracts.Leadership;

public record OfficerRequest(
    string Title,
    string? Name,
    DateTimeOffset DateStarted
);
