using BHS.Contracts.Leadership;
using BHS.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers
{
    [ApiController]
    [Route("api/leadership")]
    public class LeadershipController : ControllerBase
    {
        private readonly ILeadershipService _leadershipService;

        public LeadershipController(ILeadershipService leadershipService)
        {
            _leadershipService = leadershipService;
        }

        [HttpGet("officers")]
        public async Task<ActionResult<IList<Officer>>> GetOfficers()
            => Ok(await _leadershipService.GetOfficers());

        [HttpGet("directors")]
        public async Task<ActionResult<IList<Director>>> GetDirectors()
            => Ok(await _leadershipService.GetDirectors());
    }
}
