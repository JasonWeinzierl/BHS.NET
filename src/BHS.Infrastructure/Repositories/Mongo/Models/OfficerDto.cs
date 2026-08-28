using BHS.Contracts.Leadership;
using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record OfficerDto(
    string? Name,
    DateTimeOffset DateStarted);

internal sealed record OfficerPositionDto(
    [property: BsonId] string Title,
    int SortOrder,
    IReadOnlyCollection<OfficerDto> PositionHolders);

internal sealed record OfficeProjectionDto(
    [property: BsonId] string Title,
    int SortOrder)
{
    public Office ToOffice() => new(Title, SortOrder);
}

internal sealed record OfficerPositionUnwoundDto(
    [property: BsonId] string Title,
    int SortOrder,
    OfficerDto PositionHolders);
