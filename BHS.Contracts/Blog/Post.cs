using System;
using System.Collections.Generic;

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
        DateTimeOffset DateLastModified,
        ICollection<Category> Categories);
}
