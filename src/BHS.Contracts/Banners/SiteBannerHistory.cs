namespace BHS.Contracts.Banners;

public record SiteBannerHistory(
    string Id,
    AlertTheme Theme,
    string? Lead,
    string? Body,
    IEnumerable<SiteBannerStatusChange> StatusChanges);
