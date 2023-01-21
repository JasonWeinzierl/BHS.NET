using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Domain;
using BHS.Domain.Banners;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class SiteBannerRepository : ISiteBannerRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public SiteBannerRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<IReadOnlyCollection<SiteBanner>> GetEnabled(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<SiteBannerRevisionDto>("bannerRevisions")
            .Aggregate()
            .Match(x => x.DateModified <= _dateTimeOffsetProvider.Now())
            .SortBy(x => x.DateModified)
            .Group(x => x.RevisionId, x => new
            {
                x.Last().IsEnabled,
                x.Last().Banner,
            })
            .Match(x => x.IsEnabled)
            .Project(x => new SiteBanner((AlertTheme)x.Banner.ThemeId, x.Banner.Lead, x.Banner.Body))
            .ToListAsync(cancellationToken);
}
