using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record SiteBannerDto(
    byte ThemeId,
    string? Lead,
    string? Body);

internal sealed record SiteBannerRevisionDto(
    [property: BsonId] ObjectId RevisionId,
    ObjectId BannerId,
    DateTimeOffset DateModified,
    bool IsEnabled,
    SiteBannerDto Banner);
