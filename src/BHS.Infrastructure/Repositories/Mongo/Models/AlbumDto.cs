using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record AlbumDto(
    ObjectId Id,
    string Slug,
    string Name,
    string? Description,
    PhotoDto? BannerPhoto,
    string? BlogPostSlug,
    AuthorDto? Contributor,
    IReadOnlyCollection<PhotoDto> Photos);
