namespace BHS.Contracts.Banners;

public record SiteBannerRequest(
    AlertTheme Theme,
    string? Lead = null,
    string? Body = null,
    DateTimeOffset? EndDate = null);
