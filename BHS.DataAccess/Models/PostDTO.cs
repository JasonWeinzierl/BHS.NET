using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BHS.DataAccess.Models
{
    public record PostDTO(
        string Slug,
        string Title,
        string ContentMarkdown,
        Uri? FilePath,
        int? PhotosAlbumId,
        int? AuthorId,
        DateTimeOffset DatePublished,
        DateTimeOffset DateLastModified)
    {
        public Post ToDomainModel(IEnumerable<CategoryDTO> categories)
            => new(Slug, Title, ContentMarkdown, FilePath, PhotosAlbumId, AuthorId, DatePublished, DateLastModified, categories.Select(c => c.ToDomainModel()).ToList());
    }
}
