using BHS.Contracts.Banners;
using BHS.Domain.Banners;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Models;

namespace BHS.Infrastructure.Repositories;

public class SiteBannerRepository : ISiteBannerRepository
{
    protected IDbExecuter E { get; }

    public SiteBannerRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<IReadOnlyCollection<SiteBanner>> GetEnabled(CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<SiteBannerDTO>(DbConstants.BhsConnectionStringName, "[banners].[SiteBanner_GetEnabled]", cancellationToken: cancellationToken);
        return results.Select(r => r.ToDomainModel()).ToList();
    }
}
