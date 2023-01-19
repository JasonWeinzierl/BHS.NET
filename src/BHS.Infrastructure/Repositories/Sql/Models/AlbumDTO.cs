using BHS.Contracts;
using BHS.Contracts.Photos;

namespace BHS.Infrastructure.Repositories.Sql.Models;

public record AlbumDto(
    string Slug,
    string Name,
    string? Description,

    int? BannerPhotoId,
    string? BannerPhotoName,
    Uri? BannerPhotoImagePath,
    DateTimeOffset? BannerPhotoDatePosted,
    int? BannerPhotoAuthorId,
    string? BannerPhotoDescription,

    string? BlogPostSlug,

    int? AuthorId,
    string? AuthorDisplayName,
    string? AuthorName)
{
    public Album ToDomainModel()
    {
        Author? author = null;
        if (AuthorId.HasValue && AuthorDisplayName is not null)
            author = new Author(AuthorId.Value, AuthorDisplayName, AuthorName);
        Photo? bannerPhoto = null;
        if (BannerPhotoId.HasValue && BannerPhotoImagePath is not null && BannerPhotoDatePosted.HasValue)
            bannerPhoto = new Photo(BannerPhotoId.Value, BannerPhotoName, BannerPhotoImagePath, BannerPhotoDatePosted.Value, BannerPhotoAuthorId, BannerPhotoDescription);
        return new(Slug, Name, Description, bannerPhoto, BlogPostSlug, author);
    }

    public AlbumPhotos ToDomainModel(IEnumerable<Photo> photos)
    {
        Author? author = null;
        if (AuthorId.HasValue && AuthorDisplayName is not null)
            author = new Author(AuthorId.Value, AuthorDisplayName, AuthorName);
        Photo? bannerPhoto = null;
        if (BannerPhotoId.HasValue && BannerPhotoImagePath is not null && BannerPhotoDatePosted.HasValue)
            bannerPhoto = new Photo(BannerPhotoId.Value, BannerPhotoName, BannerPhotoImagePath, BannerPhotoDatePosted.Value, BannerPhotoAuthorId, BannerPhotoDescription);
        return new(Slug, Name, Description, bannerPhoto, BlogPostSlug, author, photos);
    }
}
