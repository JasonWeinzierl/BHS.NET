using BHS.Contracts;
using BHS.Contracts.Photos;
using System;
using System.Collections.Generic;

namespace BHS.DataAccess.Models
{
    public record AlbumDTO(
        string Slug,
        string Name,
        string? Description,

        int? BannerPhotoId,
        string? BannerPhotoName,
        Uri? BannerPhotoImagePath,
        DateTimeOffset? BannerPhotoDatePosted,
        int? BannerPhotoAuthorId,

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
            if (BannerPhotoId.HasValue)
                bannerPhoto = new Photo(BannerPhotoId.Value, BannerPhotoName, BannerPhotoImagePath, BannerPhotoDatePosted, BannerPhotoAuthorId);
            return new(Slug, Name, Description, bannerPhoto, BlogPostSlug, author);
        }

        public AlbumPhotos ToDomainModel(IEnumerable<Photo> photos)
        {
            Author? author = null;
            if (AuthorId.HasValue && AuthorDisplayName is not null)
                author = new Author(AuthorId.Value, AuthorDisplayName, AuthorName);
            Photo? bannerPhoto = null;
            if (BannerPhotoId.HasValue)
                bannerPhoto = new Photo(BannerPhotoId.Value, BannerPhotoName, BannerPhotoImagePath, BannerPhotoDatePosted, BannerPhotoAuthorId);
            return new(Slug, Name, Description, bannerPhoto, BlogPostSlug, author, photos);
        }
    }
}
