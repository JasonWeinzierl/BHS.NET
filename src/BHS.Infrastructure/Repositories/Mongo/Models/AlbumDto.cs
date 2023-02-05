using BHS.Contracts;
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
    string? AuthorUsername)
{
    public Album ToAlbum()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, AuthorUsername is null ? null : new(AuthorUsername, null));
}

internal sealed record AlbumPhotosDto(
    string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    string? AuthorUsername,
    IReadOnlyCollection<PhotoDto> Photos) : AlbumDto(Slug, Name, Description, BannerPhoto, BlogPostSlug, AuthorUsername)
{
    public AlbumPhotos ToAlbumPhotos(string? authorDisplayName) // TODO: this and ToAlbum lose the author's display name.  need de-normalize author and store the whole object within albums.
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, AuthorUsername is null ? null : new Author(AuthorUsername, authorDisplayName), Photos.Select(x => x.ToPhoto()).ToList());

    public static AlbumPhotosDto FromAlbumPhotos(AlbumPhotos albumPhotos)
        => new(
            albumPhotos.Slug,
            albumPhotos.Name,
            albumPhotos.Description,
            PhotoDto.FromPhoto(albumPhotos.BannerPhoto),
            albumPhotos.BlogPostSlug,
            albumPhotos.Author?.Username,
            albumPhotos.Photos.Select(x => PhotoDto.FromPhoto(x)).ToList());
}

internal sealed record PhotoDto(
    ObjectId Id,
    int LegacyId,
    string? Name,
    string ImagePath,
    DateTimeOffset DatePosted,
    string? AuthorUsername,
    string? Description)
{
    public Photo ToPhoto()
        => new(Id.ToString(), LegacyId, Name, new Uri(ImagePath), DatePosted, AuthorUsername, Description);

    [return: NotNullIfNotNull(nameof(photo))]
    public static PhotoDto? FromPhoto(Photo? photo)
        => photo is null ? null : new PhotoDto(ObjectId.Parse(photo.Id), photo.LegacyId, photo.Name, photo.ImagePath.ToString(), photo.DatePosted, photo.AuthorUsername, photo.Description);
}

internal sealed record UnwoundPhotosDto(IReadOnlyCollection<PhotoDto> Photos);

internal sealed record UnwoundPhotoDto(PhotoDto Photos);
