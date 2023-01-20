using BHS.Contracts.Photos;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

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
