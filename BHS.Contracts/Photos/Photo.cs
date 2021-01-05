using System;

namespace BHS.Contracts.Photos
{
    public record Photo(
        int Id,
        string Name,
        Uri ImageUri,
        DateTimeOffset DatePosted,
        int? AuthorId);
}
