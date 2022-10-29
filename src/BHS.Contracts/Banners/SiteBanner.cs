namespace BHS.Contracts.Banners;

public record SiteBanner(
    AlertTheme Theme,
    string? Lead,
    string? Body);
