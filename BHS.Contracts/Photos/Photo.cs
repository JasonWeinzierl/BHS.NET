using System;

namespace BHS.Contracts.Photos
{
    public record Photo(
        int Id,
        string Name,
        Uri ImageUri,
        bool IsVisible,
        DateTimeOffset DatePosted,
        int? AuthorId);
}
