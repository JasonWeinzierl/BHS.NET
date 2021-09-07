namespace BHS.Contracts.Blog
{
    public record Post(
        string Slug,
        string Title,
        string ContentMarkdown,
        Uri? FilePath,
        string? PhotosAlbumSlug,
        Author? Author,
        DateTimeOffset DatePublished,
        DateTimeOffset DateLastModified,
        ICollection<Category> Categories);
}
