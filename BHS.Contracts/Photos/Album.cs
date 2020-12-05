using System;

namespace BHS.Contracts.Photos
{
    public record Album(
        int Id,
        string Name,
        string Description,
        int? BannerPhotoId,
        int? BlogPostId,
        bool IsVisible,
        DateTimeOffset DateUpdated,
        int? AuthorId);
}
