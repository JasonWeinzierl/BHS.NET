using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record OfficerDto(
    string? Name,
    DateTimeOffset DateStarted);

internal sealed record OfficerPositionDto(
    [property: BsonId] string Title,
    int SortOrder,
    IReadOnlyCollection<OfficerDto> PositionHolders);

internal sealed record OfficerPositionUnwoundDto(
    [property: BsonId] string Title,
    int SortOrder,
    OfficerDto PositionHolders);
