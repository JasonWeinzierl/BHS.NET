using EphemeralMongo;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace BHS.Api.IntegrationTests;

public sealed class BhsWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly IMongoRunner _mongoRunner;
    private readonly MongoUrl _mongoUrl;
    private bool _disposedValue;

    public BhsWebApplicationFactory()
    {
        _mongoRunner = MongoRunner.Run(new MongoRunnerOptions
        {
            Version = MongoVersion.V6, // Must align with terraform-managed MongoDB version.
            MongoPort = 27018,
            StandardOutputLogger = null,
        });

        _mongoUrl = MongoUrl.Create(_mongoRunner.ConnectionString);

        // Ensure the database exists.
        var client = new MongoClient(_mongoUrl);
        client.GetDatabase("bhs").CreateCollection("logs");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Replace any occurrence of the MongoDB connection string.
                { "ConnectionStrings:bhsMongo", _mongoUrl.ToString() },
            });
        });

        builder.ConfigureServices(services =>
        {
            // Disable auth for testing.
            services.RemoveAll<IPolicyEvaluator>();
            services.AddSingleton<IPolicyEvaluator, NoAuthEvaluator>();

            // SendGrid healthcheck is more appropriate for smoke tests, not integration tests.
            services.PostConfigure<HealthCheckServiceOptions>(opt =>
            {
                var sendGridHealthCheck = opt.Registrations.Single(x => x.Name == "sendgrid");
                opt.Registrations.Remove(sendGridHealthCheck);
            });
        });

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _mongoRunner.Dispose();
            }

            _disposedValue = true;
        }

        base.Dispose(disposing);
    }
}
