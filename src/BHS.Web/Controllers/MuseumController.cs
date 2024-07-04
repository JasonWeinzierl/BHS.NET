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
    {
        var schedule = await _scheduleRepo.GetSchedule(cancellationToken);
        if (schedule is null) return NoContent();
        else return Ok(schedule);
    }

    /// <summary>
    /// Update the museum schedule.
    /// </summary>
    [HttpPut("schedule")]
    [Authorize(AuthConfig.MuseumWriteAccess)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<MuseumSchedule>> UpdateSchedule(MuseumSchedule schedule, CancellationToken cancellationToken = default)
    {
        var updatedSchedule = await _scheduleRepo.UpdateSchedule(schedule, cancellationToken);
        return CreatedAtAction(nameof(GetSchedule), updatedSchedule);
    }
}
