namespace CommandsHeadquarters.Services;

using CommandsHeadquarters.Configuraion;
using CommandsHeadquarters.Models;
using MongoDB.Bson;
using MongoDB.Driver;

public class MongoService
{
    private readonly IMongoDatabase _database;

    public MongoService(MongoConfiguration configuration)
    {
        var client = new MongoClient(configuration.ConnectionString);

        _database = client.GetDatabase(configuration.DatabaseName);
    }

    public async Task InsertAsync(string collectionName, BsonDocument alert)
    {
        var collection = _database.GetCollection<BsonDocument>(collectionName);

        await collection.InsertOneAsync(alert);
    }
}
