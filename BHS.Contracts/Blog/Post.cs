using System;

namespace BHS.Contracts.Blog
{
    public record Post(
        string Slug,
        string Title,
        string ContentMarkdown,
        Uri? FilePath,
        int? PhotosAlbumId,
        int? AuthorId,
        DateTimeOffset DatePublished,
        DateTimeOffset DateLastModified);
}
