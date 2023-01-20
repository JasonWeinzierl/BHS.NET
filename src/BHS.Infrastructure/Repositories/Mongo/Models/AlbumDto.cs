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
