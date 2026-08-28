using BHS.Contracts.Leadership;
using BHS.Domain.Leadership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/leadership")]
public class LeadershipController(
    ILeadershipRepository leadershipRepo
) : ControllerBase
{
    private readonly ILeadershipRepository _leadershipRepo = leadershipRepo;

    /// <summary>
    /// Get all officers.
    /// </summary>
    [HttpGet("officers")]
    public async Task<ActionResult<IList<Officer>>> GetOfficers(CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.GetCurrentOfficers(cancellationToken));

    [HttpGet("offices")]
    public async Task<ActionResult<IList<Office>>> GetOffices(CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.GetOffices(cancellationToken));

    /// <summary>
    /// Creates a new office.
    /// </summary>
    [HttpPost("offices")]
    [Authorize(AuthConfig.LeadershipWriteAccess)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateOffice(OfficeRequest office, CancellationToken cancellationToken = default)
    {
        await _leadershipRepo.CreateOffice(office, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Replaces officers.
    /// </summary>
    [HttpPut("officers")]
    [Authorize(AuthConfig.LeadershipWriteAccess)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IList<Officer>>> UpdateOfficers(IReadOnlyCollection<OfficerRequest> officers, CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.UpdateOfficers(officers, cancellationToken));

    /// <summary>
    /// Deletes an office. Destructive.
    /// </summary>
    [HttpDelete("offices/{title}")]
    [Authorize(AuthConfig.LeadershipWriteAccess)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOffice(string title, CancellationToken cancellationToken = default)
    {
        bool deleted = await _leadershipRepo.DeleteOffice(title, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Get current directors.
    /// </summary>
    [HttpGet("directors")]
    public async Task<ActionResult<IList<Director>>> GetDirectors(CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.GetCurrentDirectors(cancellationToken));

    /// <summary>
    /// Inserts directors.
    /// </summary>
    [HttpPost("directors")]
    [Authorize(AuthConfig.LeadershipWriteAccess)]
    public async Task<ActionResult<IList<Director>>> AddDirectors(IReadOnlyCollection<DirectorRequest> directors, CancellationToken cancellationToken = default)
        => Ok(await _leadershipRepo.AddDirectors(directors, cancellationToken));

    /// <summary>
    /// Deletes a director. Destructive.
    /// </summary>
    [HttpDelete("directors/{year:int}/{name}")]
    [Authorize(AuthConfig.LeadershipWriteAccess)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteDirector(string name, int year, CancellationToken cancellationToken = default)
    {
        bool deleted = await _leadershipRepo.DeleteDirector(name, year, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
