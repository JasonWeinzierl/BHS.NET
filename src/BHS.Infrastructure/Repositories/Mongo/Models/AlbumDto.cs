using BHS.Contracts.Photos;
using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal record AlbumDto(
    [property: BsonId] string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    AuthorDto? Contributor)
{
    public Album ToAlbum()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, Contributor?.ToAuthor());
}

internal sealed record AlbumPhotosDto(
    string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    AuthorDto? Contributor,
    IReadOnlyCollection<PhotoDto> Photos) : AlbumDto(Slug, Name, Description, BannerPhoto, BlogPostSlug, Contributor)
{
    public AlbumPhotos ToAlbumPhotos()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, Contributor?.ToAuthor(), Photos.Select(x => x.ToPhoto()).ToList());
}

internal sealed record PhotoDto(
    int Id,
    string? Name,
    string ImagePath,
    DateTimeOffset DatePosted,
    AuthorDto? Contributor,
    string? Description)
{
    public Photo ToPhoto()
        => new(Id, Name, new Uri(ImagePath), DatePosted, 0, Description); // TODO: zeros for author id!
}

internal sealed record UnwoundPhotosDto(IReadOnlyCollection<PhotoDto> Photos);

internal sealed record UnwoundPhotoDto(PhotoDto Photos);
