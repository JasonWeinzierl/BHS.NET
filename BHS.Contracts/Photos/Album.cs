namespace BHS.Contracts.Photos
{
    public record Album(
        string Slug,
        string Name,
        string? Description,
        Photo? BannerPhoto,
        string? BlogPostSlug,
        Author? Author);
}
