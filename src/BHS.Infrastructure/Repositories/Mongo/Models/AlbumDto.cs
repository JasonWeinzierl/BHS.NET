using BHS.Contracts.Photos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal record AlbumDto(
    [property: BsonId] string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    [property: Obsolete("Will be removed in a future release")] string? AuthorUsername,
    AuthorDto? Author)
{
    public Album ToAlbum()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, Author?.ToAuthor());
}

internal sealed record AlbumPhotosDto(
    string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    string? AuthorUsername,
    AuthorDto? Author,
    IReadOnlyCollection<PhotoDto> Photos) : AlbumDto(Slug, Name, Description, BannerPhoto, BlogPostSlug, AuthorUsername, Author)
{
    public AlbumPhotos ToAlbumPhotos()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, Author?.ToAuthor(), Photos.Select(x => x.ToPhoto()).ToList());

    public static AlbumPhotosDto FromAlbumPhotos(AlbumPhotos albumPhotos)
        => new(
            albumPhotos.Slug,
            albumPhotos.Name,
            albumPhotos.Description,
            PhotoDto.FromPhoto(albumPhotos.BannerPhoto),
            albumPhotos.BlogPostSlug,
            albumPhotos.Author?.Username,
            AuthorDto.FromAuthor(albumPhotos.Author),
            albumPhotos.Photos.Select(x => PhotoDto.FromPhoto(x)).ToList());
}

[BsonIgnoreExtraElements] // TODO: remove this once LegacyId is cleaned up in database.
internal sealed record PhotoDto(
    ObjectId Id,
    string? Name,
    string ImagePath,
    DateTimeOffset DatePosted,
    [property: Obsolete("Will be removed in a future release")] string? AuthorUsername,
    AuthorDto? Author,
    string? Description)
{
    public Photo ToPhoto()
        => new(Id.ToString(), Name, new Uri(ImagePath), DatePosted, Author?.ToAuthor(), Description);

    [return: NotNullIfNotNull(nameof(photo))]
    public static PhotoDto? FromPhoto(Photo? photo)
        => photo is null ? null : new PhotoDto(ObjectId.Parse(photo.Id), photo.Name, photo.ImagePath.ToString(), photo.DatePosted, photo.Author?.Username, AuthorDto.FromAuthor(photo.Author), photo.Description);
}

internal sealed record UnwoundPhotosDto(IReadOnlyCollection<PhotoDto> Photos);

internal sealed record UnwoundPhotoDto(PhotoDto Photos);
