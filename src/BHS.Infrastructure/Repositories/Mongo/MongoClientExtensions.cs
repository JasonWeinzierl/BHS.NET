using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

internal static class MongoClientExtensions
{
    public static IMongoCollection<T> GetBhsCollection<T>(this IMongoClient mongoClient, string collectionName)
        => mongoClient.GetDatabase(DbConstants.BhsMongoDatabase).GetCollection<T>(collectionName);
}
