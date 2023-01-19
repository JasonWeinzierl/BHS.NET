using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record AuthorDto(ObjectId Id, string UserName, string DisplayName);
