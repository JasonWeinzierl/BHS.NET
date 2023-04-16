using System.ComponentModel.DataAnnotations;

namespace BHS.Contracts.Blog;

public record PostRequest(
    string Title,
    string ContentMarkdown,
    [Url] Uri? FilePath,
    string? PhotosAlbumSlug,
    Author? Author,
    DateTimeOffset DatePublished,
    IEnumerable<Category> Categories);
