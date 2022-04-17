using BHS.Contracts.Banners;

namespace BHS.Domain.Banners
{
    public interface ISiteBannerRepository
    {
        Task<IReadOnlyCollection<SiteBanner>> GetEnabled();
    }
}
