namespace BHS.Contracts.Banners;

public record SiteBanner(
    string Id,
    AlertTheme Theme,
    string? Lead,
    string? Body);
