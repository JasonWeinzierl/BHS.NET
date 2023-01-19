using BHS.Contracts.Photos;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record PhotoDto(
    string? Name,
    string ImagePath,
    DateTimeOffset DatePosted,
    AuthorDto? Contributor,
    string? Description)
{
    public Photo ToPhoto()
        => new(0, Name, new Uri(ImagePath), DatePosted, 0, Description); // TODO: zeros for ids!
}
