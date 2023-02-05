using BHS.Contracts.Leadership;
using BHS.Domain.Leadership;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/leadership")]
public class LeadershipController : ControllerBase
{
    private readonly ILeadershipRepository _leadershipRepo;

    public LeadershipController(ILeadershipRepository leadershipRepo)
    {
        _leadershipRepo = leadershipRepo;
    }

    /// <summary>
    /// Get all officers.
    /// </summary>
    [HttpGet("officers")]
    public async Task<ActionResult<IList<Officer>>> GetOfficers(CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.GetCurrentOfficers(cancellationToken));

    /// <summary>
    /// Get current directors.
    /// </summary>
    [HttpGet("directors")]
    public async Task<ActionResult<IList<Director>>> GetDirectors(CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.GetCurrentDirectors(cancellationToken));
}
