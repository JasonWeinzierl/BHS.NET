using BHS.Contracts.Banners;

namespace BHS.Domain.Banners;

public interface ISiteBannerRepository
{
    Task<IReadOnlyCollection<SiteBanner>> GetEnabled(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SiteBannerHistory>> GetAllHistory(CancellationToken cancellationToken = default);
    Task<SiteBanner> Insert(SiteBannerRequest request, CancellationToken cancellationToken = default);
    Task<bool> Delete(string id, CancellationToken cancellationToken = default);
}
