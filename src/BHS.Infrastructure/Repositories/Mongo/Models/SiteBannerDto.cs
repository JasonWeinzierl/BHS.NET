using BHS.Contracts;
using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record SiteBannerDto(
    ObjectId Id,
    byte ThemeId,
    string? Lead,
    string? Body,
    IReadOnlyCollection<SiteBannerStatusChangeDto> StatusChanges)
{
    public static SiteBannerDto New(DateTimeOffset now, AlertTheme theme, string? lead, string? body, bool isEnabled)
        => new(ObjectId.GenerateNewId(now.UtcDateTime), (byte)theme, lead, body, [new SiteBannerStatusChangeDto(now, isEnabled)]);
}

internal sealed record SiteBannerUnwoundDto(
    ObjectId Id,
    byte ThemeId,
    string? Lead,
    string? Body,
    SiteBannerStatusChangeDto StatusChanges);

internal sealed record SiteBannerStatusChangeDto(
    DateTimeOffset DateModified,
    bool IsEnabled);
