using System;

namespace BHS.Contracts.Photos
{
    public record Album(
        int Id,
        string Name,
        string Description,
        int? BannerPhotoId,
        string BlogPostSlug,
        int? AuthorId);
}
