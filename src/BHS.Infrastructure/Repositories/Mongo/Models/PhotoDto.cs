namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record PhotoDto(
    string? Name,
    string ImagePath,
    DateTimeOffset DatePosted,
    AuthorDto? Contributor,
    string? Description);
