using System.Collections.Generic;

namespace BHS.Contracts.Photos
{
    public record AlbumPhotos(
        string Slug,
        string Name,
        string? Description,
        Photo? BannerPhoto,
        string? BlogPostSlug,
        Author? Author,
        IEnumerable<Photo> Photos)
        : Album(Slug, Name, Description, BannerPhoto, BlogPostSlug, Author);
}
