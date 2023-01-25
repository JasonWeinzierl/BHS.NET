using BHS.Contracts.Photos;
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
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, AuthorUsername is null ? null : new(0, AuthorUsername, null)); // TODO: author
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
        => new(Slug, Name, Description, BannerPhoto?.ToPhoto(), BlogPostSlug, AuthorUsername is null ? null : new(0, AuthorUsername, null), Photos.Select(x => x.ToPhoto()).ToList()); // TODO: author

    public static AlbumPhotosDto FromAlbumPhotos(AlbumPhotos albumPhotos)
        => new(albumPhotos.Slug, albumPhotos.Name, albumPhotos.Description, PhotoDto.FromPhoto(albumPhotos.BannerPhoto), albumPhotos.BlogPostSlug, albumPhotos.Author?.DisplayName, albumPhotos.Photos.Select(x => PhotoDto.FromPhoto(x)).ToList());
}

internal sealed record PhotoDto(
    int Id,
    string? Name,
    string ImagePath,
    DateTimeOffset DatePosted,
    string? AuthorUsername,
    string? Description)
{
    public Photo ToPhoto()
        => new(Id, Name, new Uri(ImagePath), DatePosted, 0, Description); // TODO: zeros for author id!

    [return: NotNullIfNotNull(nameof(photo))]
    public static PhotoDto? FromPhoto(Photo? photo)
        => photo is null ? null : new PhotoDto(photo.Id, photo.Name, photo.ImagePath.ToString(), photo.DatePosted, null, photo.Description); // TODO: author is lost! need to add username to contract and sql!
}

internal sealed record UnwoundPhotosDto(IReadOnlyCollection<PhotoDto> Photos);

internal sealed record UnwoundPhotoDto(PhotoDto Photos);
