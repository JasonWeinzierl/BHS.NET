namespace BHS.Contracts.Photos
{
    public record Album(
        string Slug,
        string? Name,
        string? Description,
        int? BannerPhotoId,
        string? BlogPostSlug,
        int? AuthorId);
}
