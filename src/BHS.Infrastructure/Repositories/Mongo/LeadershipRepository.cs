using BHS.Contracts.Leadership;
using BHS.Domain;
using BHS.Domain.Leadership;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

internal sealed record DirectorDto(ObjectId Id, string Name, int Year);

internal sealed record OfficerPositionDto(ObjectId Id, DateTimeOffset DateStarted, string Name, string Title, int SortOrder);

public class LeadershipRepository : ILeadershipRepository
{
    private readonly IMongoDatabase _db;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public LeadershipRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _db = mongoClient.GetDatabase("bhs");
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default)
    {
        var collection = _db.GetCollection<DirectorDto>("directors");

        var results = collection.Aggregate()
            .Match(x => x.Year > _dateTimeOffsetProvider.CurrentYear())
            .Project(x => new Director(x.Name, x.Year.ToString()));

        return await results.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
    {
        var collection = _db.GetCollection<OfficerPositionDto>("officers");

        var results = collection.Aggregate()
            .SortBy(x => x.DateStarted)
            .Group(x => x.Title, x => new
            {
                Title = x.Key,
                x.Last().Name,
                x.Last().DateStarted,
                x.Last().SortOrder,
            })
            .SortBy(x => x.SortOrder)
            .Project(x => new Officer(x.Title, x.Name, x.DateStarted));

        return await results.ToListAsync(cancellationToken);
    }
}
