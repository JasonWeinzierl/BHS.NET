using BHS.Contracts.Banners;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Domain.Repositories;

namespace BHS.DataAccess.Repositories
{
    public class SiteBannerRepository : ISiteBannerRepository
    {
        protected IDbExecuter E { get; }

        public SiteBannerRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public async Task<IReadOnlyCollection<SiteBanner>> GetEnabled()
        {
            var results = await E.ExecuteSprocQuery<SiteBannerDTO>(DbConstants.BhsConnectionStringName, "[banners].[SiteBanner_GetEnabled]");
            return results.Select(r => r.ToDomainModel()).ToList();
        }
    }
}
