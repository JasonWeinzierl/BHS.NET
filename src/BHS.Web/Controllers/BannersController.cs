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

    /// <summary>
    /// Create a new site banner.
    /// </summary>
    [HttpPost("")]
    [Authorize(AuthConfig.BannerWriteAccess)]
    public async Task<ActionResult<SiteBannerHistory>> Create(SiteBannerRequest request, CancellationToken cancellationToken = default)
    {
        var banner = await _bannerRepo.Insert(request, cancellationToken);
        return Ok(banner);
    }

    /// <summary>
    /// Deletes a site banner.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(AuthConfig.BannerWriteAccess)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SiteBannerHistory>> Delete(string id, CancellationToken cancellationToken = default)
    {
        var exists = await _bannerRepo.Delete(id, cancellationToken);
        if (!exists) return NotFound();
        return NoContent();
    }
}
