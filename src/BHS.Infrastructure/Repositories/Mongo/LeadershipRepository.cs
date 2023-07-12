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
        => await _mongoClient.GetBhsCollection<DirectorDto>("directors")
            .Aggregate()
            .Match(x => x.Year > _dateTimeOffsetProvider.CurrentYear())
            .Project(x => new Director(x.Name, x.Year.ToString()))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<OfficerPositionDto>("officerPositions")
            .Aggregate()
            .Unwind<OfficerPositionDto, OfficerPositionUnwoundDto>(x => x.PositionHolders)
            .Match(x => x.PositionHolders.DateStarted <= _dateTimeOffsetProvider.Now())
            .SortBy(x => x.PositionHolders.DateStarted)
            .Group(x => x.Title, x => new
            {
                Title = x.Key,
                x.Last().SortOrder,
                x.Last().PositionHolders,
            })
            .Match(x => x.PositionHolders.Name != null)
            .SortBy(x => x.SortOrder)
            .Project(x => new Officer(x.Title, x.PositionHolders.Name!, x.PositionHolders.DateStarted))
            .ToListAsync(cancellationToken);
}
