using MongoDB.Driver;

namespace BHS.Infrastructure;

internal static class MongoClientExtensions
{
    public static IMongoCollection<T> GetBhsCollection<T>(this IMongoClient mongoClient, string collectionName)
        => mongoClient.GetDatabase(DbConstants.BhsMongoDatabase).GetCollection<T>(collectionName);
}
