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
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, AuthorUsername is null ? null : new(0, AuthorUsername, null)); // TODO: author id zero
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
    public AlbumPhotos ToAlbumPhotos()
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, AuthorUsername is null ? null : new(0, AuthorUsername, null), Photos.Select(x => x.ToPhoto()).ToList()); // TODO: author id zero

    public static AlbumPhotosDto FromAlbumPhotos(AlbumPhotos albumPhotos, IDictionary<int, string> authors)
        => new(
            albumPhotos.Slug,
            albumPhotos.Name,
            albumPhotos.Description,
            PhotoDto.FromPhoto(
                ObjectId.GenerateNewId(albumPhotos.BannerPhoto?.DatePosted.UtcDateTime ?? DateTime.UtcNow),
                albumPhotos.BannerPhoto,
                albumPhotos.BannerPhoto?.AuthorId.HasValue == true ? authors[albumPhotos.BannerPhoto.AuthorId.Value] : null),
            albumPhotos.BlogPostSlug,
            albumPhotos.Author?.DisplayName,
            albumPhotos.Photos.Select(x => PhotoDto.FromPhoto(ObjectId.GenerateNewId(x.DatePosted.UtcDateTime), x, x.AuthorId.HasValue ? authors[x.AuthorId.Value] : null)).ToList());
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
        => new(LegacyId, Name, new Uri(ImagePath), DatePosted, 0, Description); // TODO: zeros for author id!

    [return: NotNullIfNotNull(nameof(photo))]
    public static PhotoDto? FromPhoto(ObjectId Id, Photo? photo, string? authorUsername)
        => photo is null ? null : new PhotoDto(Id, photo.Id, photo.Name, photo.ImagePath.ToString(), photo.DatePosted, authorUsername, photo.Description);
}

internal sealed record UnwoundPhotosDto(IReadOnlyCollection<PhotoDto> Photos);

internal sealed record UnwoundPhotoDto(PhotoDto Photos);
