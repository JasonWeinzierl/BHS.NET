using BHS.Contracts.Museum;
using BHS.Domain.Museum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/museum")]
public class MuseumController(IMuseumScheduleRepository scheduleRepo) : ControllerBase
{
    private readonly IMuseumScheduleRepository _scheduleRepo = scheduleRepo;

    /// <summary>
    /// Get the current museum schedule.
    /// </summary>
    [HttpGet("schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<MuseumSchedule>> GetSchedule(CancellationToken cancellationToken = default)
        => Ok(await _scheduleRepo.GetSchedule(cancellationToken));

    /// <summary>
    /// Update the museum schedule.
    /// </summary>
    [HttpPut("schedule")]
    [Authorize(AuthConfig.MuseumWriteAccess)]
    public async Task<ActionResult<MuseumSchedule>> UpdateSchedule(MuseumSchedule schedule, CancellationToken cancellationToken = default)
        => Ok(await _scheduleRepo.UpdateSchedule(schedule, cancellationToken));
}
