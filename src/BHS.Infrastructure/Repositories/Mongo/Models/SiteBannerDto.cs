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
    public static SiteBannerDto New(DateTimeOffset now, AlertTheme theme, string? lead, string? body, DateTimeOffset startDate, DateTimeOffset? endDate = null)
    {
        var statusChanges = new List<SiteBannerStatusChangeDto>
        {
            new(startDate, true)
        };
        
        if (endDate.HasValue)
        {
            statusChanges.Add(new(endDate.Value, false));
        }
        
        return new(ObjectId.GenerateNewId(now.UtcDateTime), (byte)theme, lead, body, statusChanges);
    }
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
