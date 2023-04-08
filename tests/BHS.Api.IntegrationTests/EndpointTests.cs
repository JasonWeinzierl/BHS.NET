using BHS.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace BHS.Api.IntegrationTests;

[Trait("Category", "Integration")]
public class EndpointTests : IClassFixture<MongoDbWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public EndpointTests(MongoDbWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_Ok()
    {
        var response = await _httpClient.GetAsync("/api/healthcheck/status");

        string content = await response.Content.ReadAsStringAsync();

        Assert.Equal("Healthy", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
