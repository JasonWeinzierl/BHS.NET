using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Domain.Banners;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class SiteBannerRepository : ISiteBannerRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly TimeProvider _timeProvider;

    public SiteBannerRepository(IMongoClient mongoClient, TimeProvider timeProvider)
    {
        _mongoClient = mongoClient;
        _timeProvider = timeProvider;
    }

    public async Task<IReadOnlyCollection<SiteBanner>> GetEnabled(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<SiteBannerDto>("banners")
            .Aggregate()
            .Unwind<SiteBannerDto, SiteBannerUnwoundDto>(x => x.StatusChanges)
            .Match(x => x.StatusChanges.DateModified <= _timeProvider.GetUtcNow())
            .SortBy(x => x.StatusChanges.DateModified)
            .Group(x => x.Id, x => new
            {
                x.Last().Id,
                x.Last().StatusChanges.IsEnabled,
                x.Last().ThemeId,
                x.Last().Lead,
                x.Last().Body,
            })
            .Match(x => x.IsEnabled)
            .Project(x => new SiteBanner(x.Id.ToString(), (AlertTheme)x.ThemeId, x.Lead, x.Body))
            .ToListAsync(cancellationToken);
}
