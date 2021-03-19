using BHS.Contracts;
using BHS.Contracts.Blog;
using System;

namespace BHS.DataAccess.Models
{
    public record PostPreviewDTO(
        string Slug,
        string Title,
        string ContentPreview,
        int? AuthorId,
        string? AuthorDisplayName,
        string? AuthorName,
        DateTimeOffset DatePublished)
    {
        public PostPreview ToDomainModel()
        {
            Author? author = null;
            if (AuthorId.HasValue && AuthorDisplayName is not null)
                author = new Author(AuthorId.Value, AuthorDisplayName, AuthorName);
            return new(Slug, Title, ContentPreview, author, DatePublished);
        }
    }
}
