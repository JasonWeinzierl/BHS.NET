namespace BHS.Contracts.Photos;

public record Photo(
    string Id,
    [property: Obsolete("This field will be removed in a future release.")]
    int LegacyId,
    string? Name,
    Uri ImagePath,
    DateTimeOffset DatePosted,
    string? AuthorUsername,
    string? Description);
