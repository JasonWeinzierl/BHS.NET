using BHS.Infrastructure.Repositories.Mongo;
using BHS.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BHS.Api.IntegrationTests;

[Trait("Category", "Integration")]
[Collection("Sequential")]
public class QueryTests : IClassFixture<BhsWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public QueryTests(BhsWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SequenceRepository_Increments()
    {
        // Create a random counter name to avoid collisions with other tests.
        string name = Guid.NewGuid().ToString();

        // Get a single value.
        long initial;
        using (var scope = _factory.Services.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ISequenceRepository>();
            initial = await repo.GetNextValue(name);
        }

        Assert.Equal(1, initial);

        // Get multiple values in parallel.
        long[] values;
        using (var scope = _factory.Services.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ISequenceRepository>();
            values = await Task.WhenAll(
                repo.GetNextValue(name),
                repo.GetNextValue(name),
                repo.GetNextValue(name));
        }

        Assert.Equal(new long[] { 2, 3, 4 }, values.Order());
    }
}
