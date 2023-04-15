using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

/// <summary>
/// Auto-incrementing sequences for any given name.
/// </summary>
public interface ISequenceRepository
{
    Task<long> GetNextValue(string name, CancellationToken cancellationToken = default);
}

public class SequenceRepository : ISequenceRepository
{
    private readonly IMongoClient _mongoClient;

    public SequenceRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<long> GetNextValue(string name, CancellationToken cancellationToken = default)
    {
        var options = new FindOneAndUpdateOptions<CounterDto, CounterDto>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true,
        };

        var result = await _mongoClient
            .GetBhsCollection<CounterDto>("counters")
            .FindOneAndUpdateAsync<CounterDto, CounterDto>(
                counter => counter.Name == name,
                Builders<CounterDto>.Update.Inc(x => x.Sequence, 1),
                options,
                cancellationToken);

        return result.Sequence;
    }

    private record CounterDto([property: BsonId] string Name, long Sequence);
}
