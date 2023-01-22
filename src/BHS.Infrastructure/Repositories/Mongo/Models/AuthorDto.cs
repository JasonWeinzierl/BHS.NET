using BHS.Contracts;
using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record AuthorDto(
    [property: BsonId] string UserName,
    string? DisplayName)
{
    public static AuthorDto? FromAuthor(Author? author)
        => author is null ? null : new (author.DisplayName, author.Name);

    public Author ToAuthor()
        => new(0, UserName, DisplayName); // TODO: zeros for ids!
}
