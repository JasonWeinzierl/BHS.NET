using BHS.Contracts.Leadership;
using BHS.Domain.Leadership;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

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
    public async Task<ActionResult<IList<Officer>>> GetOfficers(CancellationToken cancellationToken = default)
        => Ok(await _leadershipService.GetOfficers(cancellationToken));

    [HttpGet("directors")]
    public async Task<ActionResult<IList<Director>>> GetDirectors(CancellationToken cancellationToken = default)
        => Ok(await _leadershipService.GetDirectors(cancellationToken));
}
