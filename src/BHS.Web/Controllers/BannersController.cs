using BHS.Contracts.Banners;
using BHS.Domain.Banners;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/banners")]
public class BannersController : ControllerBase
{
    private ISiteBannerRepository _bannerRepo;

    public BannersController(ISiteBannerRepository bannerRepo)
    {
        _bannerRepo = bannerRepo;
    }

    /// <summary>
    /// Gets all currently enabled site banners.
    /// </summary>
    [HttpGet("current")]
    public async Task<ActionResult<IEnumerable<SiteBanner>>> GetEnabled(CancellationToken cancellationToken = default)
        => Ok(await _bannerRepo.GetEnabled(cancellationToken));
}
