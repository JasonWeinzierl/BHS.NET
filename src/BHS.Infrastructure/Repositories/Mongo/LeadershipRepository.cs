using BHS.Contracts.Leadership;
using BHS.Domain;
using BHS.Domain.Leadership;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
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

    public async Task BulkUpsertDirectors(IEnumerable<Director> directors, CancellationToken cancellationToken = default)
    {
        var fb = Builders<DirectorDto>.Filter;

        var dtos = directors.Select(x => new DirectorDto(ObjectId.GenerateNewId(new DateTime(int.Parse(x.Year), 1, 1)), x.Name, int.Parse(x.Year)));
        var models = dtos.Select(dir => new ReplaceOneModel<DirectorDto>(fb.Eq(x => x.Name, dir.Name) & fb.Eq(x => x.Year, dir.Year), dir) { IsUpsert = true });

        _ = await _mongoClient.GetBhsCollection<DirectorDto>("directors").BulkWriteAsync(models, cancellationToken: cancellationToken);
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

    public async Task BackfillPosition(string Title, int SortOrder, IEnumerable<(string? Name, DateTimeOffset DateStarted)> officers, CancellationToken cancellationToken = default)
    {
        var dto = new OfficerPositionDto(Title, SortOrder, officers.Select(x => new OfficerDto(x.Name, x.DateStarted)).ToList());

        var collection = _mongoClient.GetBhsCollection<OfficerPositionDto>("officerPositions");

        await collection.InsertOneAsync(dto, cancellationToken: cancellationToken);
    }
}
