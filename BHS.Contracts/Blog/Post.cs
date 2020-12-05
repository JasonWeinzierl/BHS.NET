using System;

namespace BHS.Contracts.Blog
{
    public record Post(
        int Id,
        string Title,
        string BodyContent,
        string FilePath,
        int? PhotosAlbumId,
        bool IsVisible,
        int AuthorId,
        DateTimeOffset PublishDate);
}
