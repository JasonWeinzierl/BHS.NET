using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Domain;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.IoC;
using BHS.Infrastructure.Repositories.Mongo.Models;
using Dapper;
using MongoDB.Driver;
using System.Linq;

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
    private readonly ISqlConnectionFactory _connFactory;
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public MigrationWorker(
        ILogger<MigrationWorker> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        IDbExecuter dbExecuter,
        ISqlConnectionFactory connFactory,
        IMongoClient mongoClient,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _dbExecuter = dbExecuter;
        _connFactory = connFactory;
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _logger.LogWarning("You should drop the MongoDB database before starting this, or else duplication may occur. " +
            "Pausing for 5 seconds...");
        await Task.Delay(5_000, stoppingToken);

        _logger.LogInformation("Starting migration.");

        await MigrateBanners(stoppingToken);
        await MigrateDirectors(stoppingToken);
        await MigrateAuthors(stoppingToken);
        await MigratePhotos(stoppingToken);
        await MigrateContactAlerts(stoppingToken);
        await MigrateOfficers(stoppingToken);
        await MigrateBlog(stoppingToken);

        _logger.LogInformation("Migration complete!");
        _hostApplicationLifetime.StopApplication();
    }

    private sealed record SqlSiteBanner(int Id, DateTimeOffset DateCreated, byte ThemeId, string? Lead, string? Body);
    private sealed record SqlSiteBannerEvent(int SiteBannerId, DateTimeOffset EventDate, bool IsEnabled);

    private async Task MigrateBanners(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigrateBanners));

        _logger.LogInformation("Migrating site banners.");

        _logger.LogInformation("Fetching banners from sql...");
        IEnumerable<SqlSiteBanner> sqlBanners;
        IEnumerable<SqlSiteBannerEvent> sqlBannerEvents;
        using (var sqlConn = _connFactory.CreateConnection("bhs"))
        {
            await sqlConn.OpenAsync(cancellationToken);
            sqlBanners = await sqlConn.QueryAsync<SqlSiteBanner>("SELECT * FROM [banners].[SiteBanner]");
            sqlBannerEvents = await sqlConn.QueryAsync<SqlSiteBannerEvent>("SELECT * FROM [banners].[SiteBannerEvent]");
        }

        var eventLookup = sqlBannerEvents.ToLookup(x => x.SiteBannerId);

        _logger.LogInformation("Upserting {Count} banners into mongo...", sqlBanners.Count());
        var mongoRepo = new Infrastructure.Repositories.Mongo.SiteBannerRepository(_mongoClient, _dateTimeOffsetProvider);
        foreach (var sqlBanner in sqlBanners)
        {
            var changes = eventLookup[sqlBanner.Id].Select(x => (x.EventDate, x.IsEnabled));
            await mongoRepo.Backfill(sqlBanner.DateCreated, new SiteBanner((AlertTheme)sqlBanner.ThemeId, sqlBanner.Lead, sqlBanner.Body), changes, cancellationToken);
        }

        _logger.LogInformation("Successfully migrated {Count} banners.", sqlBanners.Count());
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

        _logger.LogInformation("Fetching authors from sql to populate photo author usernames...");
        var sqlAuthorsRepo = new Infrastructure.Repositories.Sql.AuthorRepository(_dbExecuter);
        var authors = await sqlAuthorsRepo.GetAll(cancellationToken);

        _logger.LogInformation("Fetching all photo albums from sql...");
        var sqlRepo = new Infrastructure.Repositories.Sql.AlbumRepository(_dbExecuter);
        var albums = await sqlRepo.GetAll(cancellationToken);
        var allAlbumPhotos = await Task.WhenAll(albums.Select(async a => (await sqlRepo.GetBySlug(a.Slug))!));

        _logger.LogInformation("Upserting {Count} photo albums into mongo...", allAlbumPhotos.Length);
        var mongoRepo = new Infrastructure.Repositories.Mongo.AlbumRepository(_mongoClient);
        await mongoRepo.BulkUpsert(allAlbumPhotos, authors, cancellationToken);

        _logger.LogInformation("Successfully migrated {Count} albums.", allAlbumPhotos.Length);
    }

    private sealed record SqlContactAlert(int Id, string EmailAddress, string? Name, string? Message, DateTimeOffset? DateRequested, DateTimeOffset DateCreated);

    private async Task MigrateContactAlerts(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigrateContactAlerts));

        _logger.LogInformation("Migrating contact alerts.");

        _logger.LogInformation("Fetching alerts from sql...");
        IEnumerable<SqlContactAlert> sqlAlerts;
        using (var sqlConn = _connFactory.CreateConnection("bhs"))
        {
            await sqlConn.OpenAsync(cancellationToken);
            sqlAlerts = await sqlConn.QueryAsync<SqlContactAlert>("SELECT * FROM [dbo].[ContactAlert]");
        }

        _logger.LogInformation("Upserting {Count} alerts into mongo...", sqlAlerts.Count());
        var mongoRepo = new Infrastructure.Repositories.Mongo.ContactAlertRepository(_mongoClient, _dateTimeOffsetProvider);
        foreach (var sqlAlert in sqlAlerts)
        {
            var request = new ContactAlertRequest(sqlAlert.Name, sqlAlert.EmailAddress, sqlAlert.Message, sqlAlert.DateRequested, null);
            await mongoRepo.Backfill(sqlAlert.DateCreated, request, cancellationToken);
        }

        _logger.LogInformation("Successfully migrated {Count} alerts.", sqlAlerts.Count());
    }

    private sealed record SqlOfficer(int Id, string Name);
    private sealed record SqlPosition(int Id, string Title, int SortOrder);
    private sealed record SqlOfficerPositionStart(int Id, int? OfficerId, int PositionId, DateTimeOffset DateStarted);

    private async Task MigrateOfficers(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigrateOfficers));

        _logger.LogInformation("Migrating officers.");

        _logger.LogInformation("Fetching officers from sql...");
        IEnumerable<SqlOfficer> sqlOfficers;
        IEnumerable<SqlPosition> sqlPositions;
        IEnumerable<SqlOfficerPositionStart> sqlPositionStarts;
        using (var sqlConn = _connFactory.CreateConnection("bhs"))
        {
            await sqlConn.OpenAsync(cancellationToken);
            sqlOfficers = await sqlConn.QueryAsync<SqlOfficer>("SELECT * FROM [leadership].[Officer]");
            sqlPositions = await sqlConn.QueryAsync<SqlPosition>("SELECT * FROM [leadership].[Position]");
            sqlPositionStarts = await sqlConn.QueryAsync<SqlOfficerPositionStart>("SELECT * FROM [leadership].[OfficerPositionStart]");
        }

        var officerDict = sqlOfficers.ToDictionary(x => x.Id);

        _logger.LogInformation("Upserting {Count} positions for {Count} officers into mongo...", sqlPositions.Count(), sqlOfficers.Count());
        var mongoRepo = new Infrastructure.Repositories.Mongo.LeadershipRepository(_mongoClient, _dateTimeOffsetProvider);
        foreach (var position in sqlPositions)
        {
            var starts = sqlPositionStarts.Where(x => x.PositionId == position.Id);
            var officers = starts.Select(x => (x.OfficerId.HasValue ? officerDict[x.OfficerId.Value].Name : null, x.DateStarted));

            await mongoRepo.BackfillPosition(position.Title, position.SortOrder, officers, cancellationToken);
        }

        _logger.LogInformation("Successfully migrated {Count} positions and their officer history.", sqlPositions.Count());
    }

    private sealed record SqlCategory(string Slug, string Name);
    private sealed record SqlPost(string Slug);
    private sealed record SqlPostRevision(int Id, DateTimeOffset DateCreated, string PostSlug, string Title, string ContentMarkdown, string? FilePath, string? PhotosAlbumSlug, int? AuthorId);
    private sealed record SqlPostPublication(int RevisionId, DateTimeOffset DatePublished);
    private sealed record SqlPostDeletion(string PostSlug, DateTimeOffset DateDeleted);
    private sealed record SqlPostCategoryEvent(string CategorySlug, DateTimeOffset EventDate, string PostSlug, bool IsEnabled);

    private async Task MigrateBlog(CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(nameof(MigrateBlog));

        _logger.LogInformation("Migrating blog posts and categories.");

        _logger.LogInformation("Fetching blog from sql...");
        IEnumerable<SqlCategory> sqlCategories;
        IEnumerable<SqlPost> sqlPosts;
        IEnumerable<SqlPostRevision> sqlRevisions;
        IEnumerable<SqlPostPublication> sqlPublications;
        IEnumerable<SqlPostDeletion> sqlDeletions;
        IEnumerable<SqlPostCategoryEvent> sqlCategoryEvents;
        using (var sqlConn = _connFactory.CreateConnection("bhs"))
        {
            await sqlConn.OpenAsync(cancellationToken);
            sqlCategories = await sqlConn.QueryAsync<SqlCategory>("SELECT * FROM [blog].[Category]");
            sqlPosts = await sqlConn.QueryAsync<SqlPost>("SELECT * FROM [blog].[Post]");
            sqlRevisions = await sqlConn.QueryAsync<SqlPostRevision>("SELECT * FROM [blog].[PostRevision]");
            sqlPublications = await sqlConn.QueryAsync<SqlPostPublication>("SELECT * FROM [blog].[PostPublication]");
            sqlDeletions = await sqlConn.QueryAsync<SqlPostDeletion>("SELECT * FROM [blog].[PostDeletion]");
            sqlCategoryEvents = await sqlConn.QueryAsync<SqlPostCategoryEvent>("SELECT * FROM [blog].[PostCategoryEvent]");
        }

        _logger.LogInformation("Fetching authors for blog from sql...");
        var sqlAuthorRepo = new Infrastructure.Repositories.Sql.AuthorRepository(_dbExecuter);
        var authors = await sqlAuthorRepo.GetAll(cancellationToken);
        var authorsDict = authors.ToDictionary(x => x.Id);

        _logger.LogInformation("Building {Count} blog post models...", sqlPosts.Count());
        var postDtos = new List<PostDto>();

        var categoriesDict = sqlCategories.ToDictionary(x => x.Slug);
        var revisionsDict = sqlRevisions.ToLookup(x => x.PostSlug);
        var revisionsPublicationsDict = sqlPublications.ToLookup(x => x.RevisionId);
        var deletionsDict = sqlDeletions.ToLookup(x => x.PostSlug);
        var categoryEventsDict = sqlCategoryEvents.ToLookup(x => x.PostSlug);

        foreach (string slug in sqlPosts.Select(x => x.Slug))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var revisions = revisionsDict[slug].Select(rev =>
            {
                var publications = revisionsPublicationsDict[rev.Id].Select(pub => new PostRevisionPublicationDto(pub.DatePublished)).ToList();

                var author = rev.AuthorId.HasValue
                    ? new AuthorDto(authorsDict[rev.AuthorId.Value].DisplayName, authorsDict[rev.AuthorId.Value].Name)
                    : null;

                return new PostRevisionDto(rev.DateCreated, rev.Title, rev.ContentMarkdown, rev.FilePath, rev.PhotosAlbumSlug, author, publications);
            }).ToList();

            var deletions = deletionsDict[slug].Select(del => new PostDeletionDto(del.DateDeleted)).ToList();

            var categories = categoryEventsDict[slug].GroupBy(ce => ce.CategorySlug).Select(group =>
            {
                var sqlCat = categoriesDict[group.Key];
                var changes = group.Select(change => new PostCategoryChangeDto(change.EventDate, change.IsEnabled)).ToList();

                return new PostCategoryHistoryDto(sqlCat.Slug, sqlCat.Name, changes);
            }).ToList();

            postDtos.Add(new PostDto(slug, revisions, deletions, categories));
        }

        _logger.LogInformation("Inserting blog into mongo...");
        var mongoRepo = new Infrastructure.Repositories.Mongo.PostRepository(_mongoClient, _dateTimeOffsetProvider);
        foreach (var postDto in postDtos)
        {
            await mongoRepo.BackfillPost(postDto, cancellationToken);
        }

        _logger.LogInformation("Successfully migrated {Count} blog posts.", postDtos.Count);
    }
}
