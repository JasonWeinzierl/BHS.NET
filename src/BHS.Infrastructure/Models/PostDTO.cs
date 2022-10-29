using BHS.Contracts;
using BHS.Contracts.Blog;

namespace BHS.Infrastructure.Models;

public record PostDto(
    string Slug,
    string Title,
    string ContentMarkdown,
    Uri? FilePath,

    string? PhotosAlbumSlug,

    int? AuthorId,
    string? AuthorDisplayName,
    string? AuthorName,

    DateTimeOffset DatePublished,
    DateTimeOffset DateLastModified)
{
    public Post ToDomainModel(IEnumerable<Category> categories)
    {
        Author? author = null;
        if (AuthorId.HasValue && AuthorDisplayName is not null)
            author = new Author(AuthorId.Value, AuthorDisplayName, AuthorName);
        return new(Slug, Title, ContentMarkdown, FilePath, PhotosAlbumSlug, author, DatePublished, DateLastModified, categories.ToList());
    }
}
