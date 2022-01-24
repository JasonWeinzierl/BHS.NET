using BHS.Contracts.Banners;
using BHS.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers
{
    [ApiController]
    [Route("api/banners")]
    public class BannersController : ControllerBase
    {
        private readonly ISiteBannerService _bannerService;

        public BannersController(ISiteBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        /// <summary>
        /// Gets all currently enabled site banners.
        /// </summary>
        [HttpGet("current")]
        public async Task<ActionResult<IEnumerable<SiteBanner>>> GetEnabled()
            => Ok(await _bannerService.GetEnabled());
    }
}
