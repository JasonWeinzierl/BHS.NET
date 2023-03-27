using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[Route("api/client-app-environment")]
[ApiController]
public class ClientAppEnvironmentController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public ClientAppEnvironmentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the environment-specific settings for the frontend application.
    /// </summary>
    [HttpGet("")]
    public ActionResult Get()
    {
        // This must match app-environment.ts.
        return Ok(new
        {
            AppInsights = new
            {
                ConnectionString = _configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"],
            },
            Auth0 = new
            {
                Domain = _configuration["AUTH0_DOMAIN"],
                ClientId = _configuration["AUTH0_CLIENT_ID"],
                AuthorizationParams = new
                {
                    Audience = _configuration["AUTH0_AUDIENCE"],
                },
            },
        });
    }
}
