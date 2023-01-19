using BHS.Contracts.Leadership;
using BHS.Domain;
using BHS.Domain.Leadership;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class LeadershipRepository : ILeadershipRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    private IMongoCollection<DirectorDto> DirectorCollection => _mongoClient.GetDatabase("bhs").GetCollection<DirectorDto>("directors");
    private IMongoCollection<OfficerPositionDto> OfficerCollection => _mongoClient.GetDatabase("bhs").GetCollection<OfficerPositionDto>("officers");

    public LeadershipRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default)
    {
        var results = DirectorCollection.Aggregate()
            .Match(x => x.Year > _dateTimeOffsetProvider.CurrentYear())
            .Project(x => new Director(x.Name, x.Year.ToString()));

        return await results.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
    {
        var results = OfficerCollection.Aggregate()
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
