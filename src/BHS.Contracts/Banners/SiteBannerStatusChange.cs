namespace BHS.Contracts.Banners;

public record SiteBannerStatusChange(
    DateTimeOffset DateModified,
    bool IsEnabled);
