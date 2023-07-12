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

    public async Task BulkUpsert(IEnumerable<Author> authors, CancellationToken cancellationToken = default)
    {
        var fb = Builders<AuthorDto>.Filter;

        var models = authors.Select(a => new ReplaceOneModel<AuthorDto>(fb.Where(x => x.Username == a.Username), AuthorDto.FromAuthor(a)) { IsUpsert = true });

        _ = await _mongoClient.GetBhsCollection<AuthorDto>("authors").BulkWriteAsync(models, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default)
    {
        var cursor = await _mongoClient.GetBhsCollection<AuthorDto>("authors")
            .FindAsync(Builders<AuthorDto>.Filter.Empty, cancellationToken: cancellationToken);
        var results = await cursor.ToListAsync(cancellationToken);

        return results.Select(x => x.ToAuthor()).ToList();
    }

    public async Task<Author?> GetByUsername(string username, CancellationToken cancellationToken = default)
    {
        var cursor = await _mongoClient.GetBhsCollection<AuthorDto>("authors")
            .FindAsync(x => x.Username == username, cancellationToken: cancellationToken);
        var result = await cursor.SingleOrDefaultAsync(cancellationToken);

        return result?.ToAuthor();
    }

    public async Task<Author> Upsert(Author author, CancellationToken cancellationToken = default)
    {
        var collection = _mongoClient.GetBhsCollection<AuthorDto>("authors");

        var dto = AuthorDto.FromAuthor(author);
        var replaceOptions = new ReplaceOptions { IsUpsert = true };

        _ = await collection.ReplaceOneAsync(x => x.Username == dto.Username, dto, replaceOptions, cancellationToken);

        return dto.ToAuthor();
    }
}
