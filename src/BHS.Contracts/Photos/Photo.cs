namespace BHS.Contracts.Photos;

public record Photo(
    int Id,
    string? Name,
    Uri ImagePath,
    DateTimeOffset DatePosted,
    [property: Obsolete("This property will be replaced in a future release.")] int? AuthorId,
    string? Description);
