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

    public LeadershipRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default)
        => await GetCollection<DirectorDto>("directors")
            .Aggregate()
            .Match(x => x.Year > _dateTimeOffsetProvider.CurrentYear())
            .Project(x => new Director(x.Name, x.Year.ToString()))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
        => await GetCollection<OfficerPositionDto>("officers")
            .Aggregate()
            .SortBy(x => x.DateStarted)
            .Group(x => x.Title, x => new
            {
                Title = x.Key,
                x.Last().Name,
                x.Last().DateStarted,
                x.Last().SortOrder,
            })
            .SortBy(x => x.SortOrder)
            .Project(x => new Officer(x.Title, x.Name, x.DateStarted))
            .ToListAsync(cancellationToken);

    private IMongoCollection<T> GetCollection<T>(string collectionName)
        => _mongoClient.GetDatabase("bhs").GetCollection<T>(collectionName);
}
