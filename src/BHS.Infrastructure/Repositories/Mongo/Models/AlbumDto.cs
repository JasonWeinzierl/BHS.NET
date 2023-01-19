using BHS.Contracts;
using BHS.Contracts.Photos;
using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal record AlbumDto(
    ObjectId Id,
    string Slug,
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
    ObjectId Id,
    string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    AuthorDto? Contributor,
    IReadOnlyCollection<PhotoDto> Photos) : AlbumDto(Id, Slug, Name, Description, BannerPhoto, BlogPostSlug, Contributor)
{
    public AlbumPhotos ToAlbumPhotos()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, Contributor?.ToAuthor(), Photos.Select(x => x.ToPhoto()).ToList());
}
