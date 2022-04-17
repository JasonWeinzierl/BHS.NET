using BHS.Contracts.Banners;

namespace BHS.Domain.Banners
{
    public class SiteBannerService : ISiteBannerService
    {
        private readonly ISiteBannerRepository _bannerRepository;

        public SiteBannerService(
            ISiteBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IReadOnlyCollection<SiteBanner>> GetEnabled()
            => await _bannerRepository.GetEnabled();
    }
}
