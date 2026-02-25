using System.ComponentModel.DataAnnotations;

namespace BHS.Contracts.Blog;

public record PostRequest(
    string Title,
    string ContentMarkdown,
    DateTimeOffset DatePublished,
    IEnumerable<Category> Categories,
    [Url] Uri? FilePath = null,
    string? PhotosAlbumSlug = null,
    Author? Author = null);
