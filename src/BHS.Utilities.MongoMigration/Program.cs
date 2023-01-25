using BHS.Contracts.Photos;
using BHS.Domain;
using BHS.Domain.Banners;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.IoC;
using BHS.Infrastructure.Providers;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Diagnostics;

namespace BHS.Utilities.MongoMigration;

internal class Program
{
    static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddBhsServices();

                services.AddHostedService<MigrationWorker>();
            })
            .RunConsoleAsync();
    }
}

internal sealed class MigrationWorker : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IDbExecuter _dbExecuter;
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public MigrationWorker(
        ILogger<MigrationWorker> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IDbExecuter dbExecuter,
        IMongoClient mongoClient,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _dbExecuter = dbExecuter;
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        _logger.LogInformation("Starting migration.");

        await MigrateMutableData(stoppingToken);

        // TODO: will need to write custom queries to fetch the immutable data.
        // await MigrateImmutableData(stoppingToken);

        _logger.LogInformation("Migration complete!");
        _hostApplicationLifetime.StopApplication();
    }

    private async Task MigrateMutableData(CancellationToken cancellationToken)
    {
        await MigrateDirectors(cancellationToken);
        await MigrateAuthors(cancellationToken);
        await MigratePhotos(cancellationToken);
    }

    private async Task MigrateDirectors(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigrateDirectors));

        var mockNow = new Moq.Mock<IDateTimeOffsetProvider>();
        mockNow.Setup(x => x.Now()).Returns(() => new DateTimeOffset(2000, 01, 01, 00, 00, 00, TimeSpan.FromHours(0)));

        _logger.LogInformation("Migrating all directors from {Date}", mockNow.Object.Now());

        _logger.LogInformation("Fetching directors from sql...");
        var sqlRepo = new Infrastructure.Repositories.Sql.LeadershipRepository(_dbExecuter, mockNow.Object);
        var directors = await sqlRepo.GetCurrentDirectors(cancellationToken);

        _logger.LogInformation("Upserting {Count} directors into mongo...", directors.Count);
        var mongoRepo = new Infrastructure.Repositories.Mongo.LeadershipRepository(_mongoClient, _dateTimeOffsetProvider);
        await mongoRepo.BulkUpsertDirectors(directors, cancellationToken);

        _logger.LogInformation("Successfully migrated {Count} directors.", directors.Count);
    }

    private async Task MigrateAuthors(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigrateAuthors));

        _logger.LogInformation("Migrating authors.");

        _logger.LogInformation("Fetching authors from sql...");
        var sqlRepo = new Infrastructure.Repositories.Sql.AuthorRepository(_dbExecuter);
        var authors = await sqlRepo.GetAll(cancellationToken);

        _logger.LogInformation("Upserting {Count} authors into mongo...", authors.Count);
        var mongoRepo = new Infrastructure.Repositories.Mongo.AuthorRepository(_mongoClient);
        await mongoRepo.BulkUpsert(authors, cancellationToken);

        _logger.LogInformation("Successfully migrated {Count} authors.", authors.Count);
    }

    private async Task MigratePhotos(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigratePhotos));

        _logger.LogInformation("Migrating photos.");

        _logger.LogInformation("Fetching all photo albums from sql...");
        var sqlRepo = new Infrastructure.Repositories.Sql.AlbumRepository(_dbExecuter);
        var albums = await sqlRepo.GetAll(cancellationToken);
        var allAlbumPhotos = await Task.WhenAll(albums.Select(async a => (await sqlRepo.GetBySlug(a.Slug))!));

        _logger.LogInformation("Upserting {Count} photo albums into mongo...", allAlbumPhotos.Length);
        var mongoRepo = new Infrastructure.Repositories.Mongo.AlbumRepository(_mongoClient);
        await mongoRepo.BulkUpsert(allAlbumPhotos, cancellationToken);

        _logger.LogInformation("Successfully migrated {Count} albums.", allAlbumPhotos.Length);
    }
}
