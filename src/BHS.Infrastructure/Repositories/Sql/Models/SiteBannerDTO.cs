using BHS.Contracts;
using BHS.Contracts.Banners;

namespace BHS.Infrastructure.Repositories.Sql.Models;

public record SiteBannerDTO(
    int Id,
    byte ThemeId,
    string? Lead,
    string? Body)
{
    public SiteBanner ToDomainModel()
        => new((AlertTheme)ThemeId, Lead, Body);
}
