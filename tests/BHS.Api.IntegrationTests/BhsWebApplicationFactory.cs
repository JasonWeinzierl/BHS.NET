using Auth0.ManagementApi;
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
using NSubstitute;

namespace BHS.Api.IntegrationTests;

public sealed class BhsWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly IMongoRunner _mongoRunner;
    private readonly MongoUrl _mongoUrl;
    private bool _disposedValue;

    public IManagementConnection MockManagementConnection { get; } = Substitute.For<IManagementConnection>();

    public BhsWebApplicationFactory()
    {
        _mongoRunner = MongoRunner.Run(new MongoRunnerOptions
        {
            MongoPort = 27018,
            StandardOuputLogger = null,

            KillMongoProcessesWhenCurrentProcessExits = true,
        });

        // Add the database to the URL (Serilog requires it).
        var builder = new MongoUrlBuilder(_mongoRunner.ConnectionString)
        {
            DatabaseName = "bhs"
        };
        _mongoUrl = builder.ToMongoUrl();

        // Ensure the database exists.
        var client = new MongoClient(_mongoUrl);
        client.GetDatabase(_mongoUrl.DatabaseName).CreateCollection("logs");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Replace any occurrence of the MongoDB connection string.
                { "ConnectionStrings:bhsMongo", _mongoUrl.ToString() },
                { "Serilog:WriteTo:0:Args:databaseUrl", _mongoUrl.ToString() },

                { "Auth0ManagementApiOptions:Domain", "test.com" },
                { "Auth0ManagementApiOptions:ClientId", "foo" },
                { "Auth0ManagementApiOptions:ClientSecret", "bar" },
            });
        });

        builder.ConfigureServices(services =>
        {
            // Disable auth for testing.
            services.RemoveAll<IPolicyEvaluator>();
            services.AddSingleton<IPolicyEvaluator, NoAuthEvaluator>();

            // Mock Auth0 management api.
            services.RemoveAll<IManagementConnection>();
            services.AddSingleton(provider => MockManagementConnection);

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
