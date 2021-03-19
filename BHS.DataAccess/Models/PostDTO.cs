using BHS.Contracts;
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
        string? AuthorDisplayName,
        string? AuthorName,
        DateTimeOffset DatePublished,
        DateTimeOffset DateLastModified)
    {
        public Post ToDomainModel(IEnumerable<CategoryDTO> categories)
        {
            Author? author = null;
            if (AuthorId.HasValue && AuthorDisplayName is not null)
                author = new Author(AuthorId.Value, AuthorDisplayName, AuthorName);
            return new(Slug, Title, ContentMarkdown, FilePath, PhotosAlbumId, author, DatePublished, DateLastModified, categories.Select(c => c.ToDomainModel()).ToList());
        }
    }
}
