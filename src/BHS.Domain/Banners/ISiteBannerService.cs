using BHS.Contracts.Banners;

namespace BHS.Domain.Banners
{
    public interface ISiteBannerService
    {
        Task<IReadOnlyCollection<SiteBanner>> GetEnabled();
    }
}
