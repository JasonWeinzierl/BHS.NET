using BHS.Contracts.Photos;

namespace BHS.DataAccess.Models
{
    public record AlbumDTO(
        int Id,
        string? Name,
        string? Description,
        int? BannerPhotoId,
        string? BlogPostSlug,
        int? AuthorId)
    {
        public Album ToDomainModel()
            => new(Id.ToString(), Name, Description, BannerPhotoId, BlogPostSlug, AuthorId);
    }
}
