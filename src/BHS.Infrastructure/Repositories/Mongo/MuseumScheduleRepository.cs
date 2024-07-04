using BHS.Contracts.Museum;
using BHS.Domain.Museum;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class MuseumScheduleRepository(
    IMongoClient mongoClient,
    TimeProvider timeProvider) : IMuseumScheduleRepository
{
    private readonly IMongoClient _mongoClient = mongoClient;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<MuseumSchedule?> GetSchedule(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<MuseumScheduleRevisionDto>("museumScheduleRevisions")
            .Aggregate()
            .Match(x => x.DateStarted <= _timeProvider.GetUtcNow())
            .SortByDescending(x => x.DateStarted)
            .Limit(1)
            .Project(x => new MuseumSchedule(x.Schedule.Days, x.Schedule.Months))
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<MuseumSchedule> UpdateSchedule(MuseumSchedule schedule, CancellationToken cancellationToken = default)
    {
        var dto = new MuseumScheduleRevisionDto(ObjectId.GenerateNewId(), _timeProvider.GetUtcNow(), schedule);
        await _mongoClient.GetBhsCollection<MuseumScheduleRevisionDto>("museumScheduleRevisions")
            .InsertOneAsync(dto, cancellationToken: cancellationToken);
        return schedule;
    }
}
