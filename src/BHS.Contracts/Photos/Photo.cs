namespace BHS.Contracts.Photos
{
    public record Photo(
        int Id,
        string? Name,
        Uri ImagePath,
        DateTimeOffset DatePosted,
        int? AuthorId);
}
