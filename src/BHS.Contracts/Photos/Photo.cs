namespace BHS.Contracts.Photos;

public record Photo(
    string Id,
    string? Name,
    Uri ImagePath,
    DateTimeOffset DatePosted,
    [property: Obsolete("Will be removed in a future release")] string? AuthorUsername,
    Author? Author,
    string? Description);
