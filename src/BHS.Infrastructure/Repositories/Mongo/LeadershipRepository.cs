using BHS.Contracts.Leadership;
using BHS.Domain;
using BHS.Domain.Leadership;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

internal sealed record DirectorDto([property: BsonId(IdGenerator = typeof(StringObjectIdGenerator))] string Id, string Name, int Year);

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

        var fb = Builders<DirectorDto>.Filter;
        var filter = fb.Gte(x => x.Year, _dateTimeOffsetProvider.CurrentYear());

        var results =  await (await collection.FindAsync(filter, cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken);

        return results.Select(x => new Director(x.Name, x.Year.ToString())).ToList();
    }

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
    {
        var collection = _db.GetCollection<Officer>("officers");

        var results = await collection.FindAsync(FilterDefinition<Officer>.Empty, cancellationToken: cancellationToken);

        return await results.ToListAsync(cancellationToken);
    }
}
