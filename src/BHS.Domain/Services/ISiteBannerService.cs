using BHS.Contracts.Banners;

namespace BHS.Domain.Services
{
    public interface ISiteBannerService
    {
        Task<IReadOnlyCollection<SiteBanner>> GetEnabled();
    }
}
