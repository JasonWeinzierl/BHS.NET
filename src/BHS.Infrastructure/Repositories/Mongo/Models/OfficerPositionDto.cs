using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record OfficerPositionDto(
    ObjectId Id,
    DateTimeOffset DateStarted,
    string Name,
    string Title,
    int SortOrder);
