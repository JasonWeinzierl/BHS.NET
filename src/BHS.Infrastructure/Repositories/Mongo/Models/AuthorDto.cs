using BHS.Contracts;
using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record AuthorDto(ObjectId Id, string UserName, string DisplayName)
{
    public Author ToAuthor()
        => new(0, UserName, DisplayName); // TODO: zeros for ids!
}
