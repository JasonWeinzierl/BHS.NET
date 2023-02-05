using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json.Serialization;

namespace BHS.Web;

internal static class HealthChecksConfig
{
    /// <summary>
    /// Adds health checks.
    /// </summary>
    public static void AddBhsHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
                .AddMongoDb(configuration.GetConnectionString("bhsMongo")!)
                .AddCheck<SendGridHealthCheck>("sendgrid");
    }
}

internal sealed class SendGridHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SendGridHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // See https://status.sendgrid.com/api for API documentation.

        using var client = _httpClientFactory.CreateClient("SendGridStatus");
        var response = await client.GetAsync("https://status.sendgrid.com/api/v2/status.json", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return HealthCheckResult.Unhealthy(response.ReasonPhrase);

        var statusJson = await response.Content.ReadFromJsonAsync<PartialSendGridStatusJson>(cancellationToken: cancellationToken);

        string? description = statusJson?.Status.Description;
        return statusJson?.Status.Indicator switch
        {
            SendGridStatusIndicator.None => HealthCheckResult.Healthy(description),
            SendGridStatusIndicator.Minor => HealthCheckResult.Degraded(description),
            _ => HealthCheckResult.Unhealthy(description),
        };
    }

    private sealed record PartialSendGridStatusJson(SendGridStatus Status);

    private sealed record SendGridStatus(SendGridStatusIndicator Indicator, string Description);

    [JsonConverter(typeof(JsonStringEnumConverter))]
    private enum SendGridStatusIndicator
    {
        None,
        Minor,
        Major,
        Critical,
    }
}
