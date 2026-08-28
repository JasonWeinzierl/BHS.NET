using BHS.Contracts.Leadership;
using BHS.Domain.Leadership;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class LeadershipRepository(
    IMongoClient mongoClient,
    TimeProvider timeProvider
) : ILeadershipRepository
{
    private readonly IMongoClient _mongoClient = mongoClient;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<DirectorDto>("directors")
            .Aggregate()
            .Match(x => x.Year >= _timeProvider.GetUtcNow().Year)
            .SortBy(x => x.Year)
            .Project(x => new Director(x.Name, x.Year.ToString()))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Director>> AddDirectors(
        IReadOnlyCollection<DirectorRequest> directors,
        CancellationToken cancellationToken = default)
    {
        if (directors.Count > 0)
        {
            var documents = directors.Select(director => new DirectorDto(
                ObjectId.GenerateNewId(),
                director.Name,
                director.Year));

            await _mongoClient
                .GetBhsCollection<DirectorDto>("directors")
                .InsertManyAsync(documents, cancellationToken: cancellationToken);
        }

        return await GetCurrentDirectors(cancellationToken);
    }

    public async Task<bool> DeleteDirector(string name, int year, CancellationToken cancellationToken = default)
    {
        var result = await _mongoClient
            .GetBhsCollection<DirectorDto>("directors")
            .DeleteManyAsync(director => director.Name == name && director.Year == year, cancellationToken);

        return result.DeletedCount > 0;
    }

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
        => await _mongoClient.GetBhsCollection<OfficerPositionDto>("officerPositions")
            .Aggregate()
            .Unwind<OfficerPositionDto, OfficerPositionUnwoundDto>(x => x.PositionHolders)
            .Match(x => x.PositionHolders.DateStarted <= _timeProvider.GetUtcNow())
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

    public async Task CreateOffice(
        OfficeRequest office,
        CancellationToken cancellationToken = default)
    {
        var document = new OfficerPositionDto(office.Title, office.SortOrder, []);
        await _mongoClient
            .GetBhsCollection<OfficerPositionDto>("officerPositions")
            .InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<Officer>> UpdateOfficers(
        IReadOnlyCollection<OfficerRequest> officers,
        CancellationToken cancellationToken = default)
    {
        var collection = _mongoClient.GetBhsCollection<OfficerPositionDto>("officerPositions");
        var officeTitles = await collection
            .Find(Builders<OfficerPositionDto>.Filter.Empty)
            .Project(position => position.Title)
            .ToListAsync(cancellationToken);

        var officerTitles = officers.Select(officer => officer.Title).ToHashSet(StringComparer.Ordinal);

        if (officerTitles.Count != officers.Count || !officerTitles.SetEquals(officeTitles))
            throw new InvalidOfficeTitlesException("Officer titles must exactly match the existing office titles.");

        if (officers.Count > 0)
        {
            var updates = officers.Select(officer => new UpdateOneModel<OfficerPositionDto>(
                Builders<OfficerPositionDto>.Filter.Eq(position => position.Title, officer.Title),
                Builders<OfficerPositionDto>.Update.Push(
                    position => position.PositionHolders,
                    new OfficerDto(officer.Name, officer.DateStarted))));

            _ = await collection.BulkWriteAsync(updates, cancellationToken: cancellationToken);
        }

        return await GetCurrentOfficers(cancellationToken);
    }

    public async Task<bool> DeleteOffice(string title, CancellationToken cancellationToken = default)
    {
        var result = await _mongoClient
            .GetBhsCollection<OfficerPositionDto>("officerPositions")
            .DeleteOneAsync(position => position.Title == title, cancellationToken);

        return result.DeletedCount > 0;
    }
}
