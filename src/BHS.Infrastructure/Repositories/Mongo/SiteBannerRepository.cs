using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Domain.Banners;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class SiteBannerRepository(IMongoClient mongoClient, TimeProvider timeProvider) : ISiteBannerRepository
{
    private readonly IMongoClient _mongoClient = mongoClient;
    private readonly TimeProvider _timeProvider = timeProvider;

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

    public async Task<IReadOnlyCollection<SiteBannerHistory>> GetAllHistory(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<SiteBannerDto>("banners")
            .Aggregate()
            .Project(x => new SiteBannerHistory(
                x.Id.ToString(),
                (AlertTheme)x.ThemeId,
                x.Lead,
                x.Body,
                x.StatusChanges.Select(x => new SiteBannerStatusChange(x.DateModified, x.IsEnabled))))
            .ToListAsync(cancellationToken);

    public async Task<SiteBanner> Insert(SiteBannerRequest request, CancellationToken cancellationToken = default)
    {
        var now = _timeProvider.GetUtcNow();
        var banner = SiteBannerDto.New(now, request.Theme, request.Lead, request.Body, now, request.EndDate);

        await _mongoClient.GetBhsCollection<SiteBannerDto>("banners")
            .InsertOneAsync(banner, cancellationToken: cancellationToken);

        return new SiteBanner(
            banner.Id.ToString(),
            request.Theme,
            request.Lead,
            request.Body);
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
    {
        var updatedBanner = await UpdateStatus(id, new SiteBannerStatusChange(_timeProvider.GetUtcNow(), false), cancellationToken);
        return updatedBanner is not null;
    }

    private async Task<SiteBannerHistory?> UpdateStatus(string id, SiteBannerStatusChange change, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(id, out var bannerObjectId))
        {
            throw new InvalidBannerIdException("The requested banner id is not formatted correctly. It must consist of 24 hexadecimal digits.");
        }

        var ub = Builders<SiteBannerDto>.Update;
        var banner = await _mongoClient.GetBhsCollection<SiteBannerDto>("banners")
            .FindOneAndUpdateAsync(
                x => x.Id == bannerObjectId,
                ub.Push(x => x.StatusChanges, new SiteBannerStatusChangeDto(change.DateModified, change.IsEnabled)),
                cancellationToken: cancellationToken);

        if (banner is null)
        {
            return null;
        }

        return new SiteBannerHistory(
            banner.Id.ToString(),
            (AlertTheme)banner.ThemeId,
            banner.Lead,
            banner.Body,
            banner.StatusChanges.Select(x => new SiteBannerStatusChange(x.DateModified, x.IsEnabled)));
    }
}
