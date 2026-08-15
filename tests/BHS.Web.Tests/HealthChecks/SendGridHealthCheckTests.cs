using Microsoft.Extensions.Diagnostics.HealthChecks;
using NSubstitute;
using System.Net;
using Xunit;

namespace BHS.Web.Tests.HealthChecks;

public class SendGridHealthCheckTests
{
    [Fact]
    public async Task CheckHealthAsync_UsesTwilioStatusEndpoint()
    {
        var handler = new RecordingHandler();
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient("SendGridStatus").Returns(new HttpClient(handler));
        var healthCheck = new SendGridHealthCheck(httpClientFactory);

        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext(), TestContext.Current.CancellationToken);

        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Equal(new Uri("https://status.twilio.com/api/v2/status.json"), handler.RequestUri);
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """{"status":{"indicator":"none","description":"All Systems Operational"}}"""),
            });
        }
    }
}
