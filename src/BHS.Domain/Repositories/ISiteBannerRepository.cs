using BHS.Contracts.Banners;

namespace BHS.Domain.Repositories
{
    public interface ISiteBannerRepository
    {
        Task<IReadOnlyCollection<SiteBanner>> GetEnabled();
    }
}
