namespace BHS.Contracts;

public record Author(
    [property: Obsolete("This property will be removed in a future release.")] int Id,
    string DisplayName,
    string? Name);
