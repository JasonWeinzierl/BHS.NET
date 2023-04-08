using EphemeralMongo;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace BHS.Api.IntegrationTests;

/// <remarks>
/// Spins up an isolated MongoDB instance alongside the test server.
/// </remarks>
/// <inheritdoc />
public sealed class MongoDbWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private const int PORT = 27018;
    private const string DATABASE = "bhs";

    private readonly IMongoRunner _mongoRunner;
    private bool _disposedValue;

    public MongoDbWebApplicationFactory()
    {
        _mongoRunner = MongoRunner.Run(new MongoRunnerOptions
        {
            MongoPort = PORT,
            StandardOuputLogger = null,

            KillMongoProcessesWhenCurrentProcessExits = true,
        });

        // Ensure the database exists.
        var client = new MongoClient($"mongodb://localhost:{PORT}/{DATABASE}");
        client.GetDatabase(DATABASE).CreateCollection("logs");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Replace any occurrence of the MongoDB connection string.
                { "ConnectionStrings:bhsMongo", $"mongodb://localhost:{PORT}/{DATABASE}" },
                { "Serilog:WriteTo:0:Args:databaseUrl", $"mongodb://localhost:{PORT}/{DATABASE}" },
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
