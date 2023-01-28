using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Domain;
using BHS.Domain.Banners;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
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
        => await _mongoClient.GetBhsCollection<SiteBannerDto>("banners")
            .Aggregate()
            .Unwind<SiteBannerDto, SiteBannerUnwoundDto>(x => x.StatusChanges)
            .Match(x => x.StatusChanges.DateModified <= _dateTimeOffsetProvider.Now())
            .SortBy(x => x.StatusChanges.DateModified)
            .Group(x => x.Id, x => new
            {
                x.Last().StatusChanges.IsEnabled,
                x.Last().ThemeId,
                x.Last().Lead,
                x.Last().Body,
            })
            .Match(x => x.IsEnabled)
            .Project(x => new SiteBanner((AlertTheme)x.ThemeId, x.Lead, x.Body))
            .ToListAsync(cancellationToken);

    public async Task Backfill(
        DateTimeOffset dateCreated,
        SiteBanner banner,
        IEnumerable<(DateTimeOffset DateModified, bool IsEnabled)> statusChanges,
        CancellationToken cancellationToken = default)
    {
        var objectId = ObjectId.GenerateNewId(dateCreated.UtcDateTime);
        var changeDtos = statusChanges.Select(x => new SiteBannerStatusChangeDto(x.DateModified, x.IsEnabled)).ToList();
        var dto = new SiteBannerDto(objectId, (byte)banner.Theme, banner.Lead, banner.Body, changeDtos);

        var collection = _mongoClient.GetBhsCollection<SiteBannerDto>("banners");

        await collection.InsertOneAsync(dto, cancellationToken: cancellationToken);
    }
}
