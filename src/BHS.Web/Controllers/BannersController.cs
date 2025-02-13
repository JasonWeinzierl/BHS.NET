using BHS.Contracts.Banners;
using BHS.Domain.Banners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/banners")]
public class BannersController(ISiteBannerRepository bannerRepo) : ControllerBase
{
    private readonly ISiteBannerRepository _bannerRepo = bannerRepo;

    /// <summary>
    /// Get all currently enabled site banners.
    /// </summary>
    [HttpGet("current")]
    public async Task<ActionResult<IEnumerable<SiteBanner>>> GetEnabled(CancellationToken cancellationToken = default)
        => Ok(await _bannerRepo.GetEnabled(cancellationToken));

    /// <summary>
    /// Get the history of all site banners.
    /// </summary>
    [HttpGet("history")]
    [Authorize(AuthConfig.BannerWriteAccess)]
    public async Task<ActionResult<IEnumerable<SiteBannerHistory>>> GetAllHistory(CancellationToken cancellationToken = default)
        => Ok(await _bannerRepo.GetAllHistory(cancellationToken));
}
