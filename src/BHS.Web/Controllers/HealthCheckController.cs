using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/healthcheck")]
public class HealthCheckController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthCheckController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// Get the aggregate status of all health checks.
    /// </summary>
    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> CheckHealthAsync(CancellationToken cancellationToken = default)
        => Ok((await _healthCheckService.CheckHealthAsync(cancellationToken)).Status.ToString());
}
