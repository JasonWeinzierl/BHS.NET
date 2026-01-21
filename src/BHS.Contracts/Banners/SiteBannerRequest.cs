namespace BHS.Contracts.Banners;

public record SiteBannerRequest(
    AlertTheme Theme,
    string? Lead,
    string? Body,
    DateTimeOffset? EndDate = null);
