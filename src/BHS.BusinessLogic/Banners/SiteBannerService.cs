using BHS.Contracts.Banners;
using BHS.Domain.Repositories;
using BHS.Domain.Services;

namespace BHS.BusinessLogic.Banners
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
