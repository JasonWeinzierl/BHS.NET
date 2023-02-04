using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/healthcheck")]
public class HealthCheckController : ControllerBase
{
    private readonly HealthCheckOptions _options;
    private readonly HealthCheckService _healthCheckService;

    public HealthCheckController(
        IOptions<HealthCheckOptions> options,
        HealthCheckService healthCheckService)
    {
        _options = options.Value;
        _healthCheckService = healthCheckService;
    }

    /// <summary>
    /// Get the aggregate status of all health checks.
    /// </summary>
    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var report = await _healthCheckService.CheckHealthAsync(_options.Predicate, cancellationToken);

        if (!_options.ResultStatusCodes.TryGetValue(report.Status, out int statusCode))
            throw new InvalidOperationException($"No health check status code was found for {report.Status}.");

        return StatusCode(statusCode, report.Status.ToString());
    }
}
