using BHS.Contracts;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record AuthorDto(
    [property: BsonId] string Username,
    string? DisplayName)
{
    [return: NotNullIfNotNull(nameof(author))]
    public static AuthorDto? FromAuthor(Author? author)
        => author is null ? null : new(author.Username, author.Name);

    public Author ToAuthor()
        => new(Username, DisplayName);
}
