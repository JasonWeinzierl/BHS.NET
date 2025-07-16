using BHS.Contracts;
using BHS.Domain.Authors;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

internal sealed record AuthorDto(
    string Id,
    string[] AuthUserId,
    string DisplayName);

public class AuthorRepository(
    IMongoClient mongoClient) : IAuthorRepository
{
    private readonly IMongoCollection<AuthorDto> _authorsCollection = mongoClient
        .GetBhsCollection<AuthorDto>("authors");

    public async Task<IReadOnlyCollection<Author>> GetByAuthUserId(string authUserId, CancellationToken cancellationToken = default)
    {
        return await _authorsCollection
            .Aggregate()
            .Match(x => x.AuthUserId.Contains(authUserId))
            .Project(x => new Author(x.Id, x.DisplayName))
            .ToListAsync(cancellationToken);
    }
}
