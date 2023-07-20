namespace BHS.Contracts.Photos;

public record Photo(
    string Id,
    string? Name,
    Uri ImagePath,
    DateTimeOffset DatePosted,
    Author? Author,
    string? Description);
