using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record DirectorDto(ObjectId Id, string Name, int Year);
