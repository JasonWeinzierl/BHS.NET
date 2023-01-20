using BHS.Contracts;
using BHS.Domain.Authors;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BHS.Infrastructure.Repositories.Mongo;

public class AuthorRepository : IAuthorRepository
{
    private readonly IMongoClient _mongoClient;

    public AuthorRepository(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default)
    {
        var cursor = await _mongoClient.GetBhsCollection<AuthorDto>("authors")
            .FindAsync(Builders<AuthorDto>.Filter.Empty, cancellationToken: cancellationToken);
        var results = await cursor.ToListAsync(cancellationToken);

        return results.Select(x => x.ToAuthor()).ToList();
    }

    public async Task<Author?> GetByUserName(string userName, CancellationToken cancellationToken = default)
    {
        var cursor = await _mongoClient.GetBhsCollection<AuthorDto>("authors")
            .FindAsync(x => x.UserName == userName, cancellationToken: cancellationToken);
        var result = await cursor.SingleOrDefaultAsync(cancellationToken);

        return result?.ToAuthor();
    }
}
