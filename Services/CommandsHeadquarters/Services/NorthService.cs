
using CommandsHeadquarters.Models;
using MongoDB.Bson;
using System.Text.Json;

namespace CommandsHeadquarters.Services;

public class NorthService
{
    private readonly MongoService _mongoService;

    public NorthService(MongoService mongoService)
    {
        _mongoService = mongoService;
    }

    public async Task ProcessAsync(string message)
    {
        Console.WriteLine($"North received: {message}");

        var newMsg = message.Replace("\'", "\"");
        var msgObj = JsonSerializer.Deserialize<Alert>(newMsg);
        var bsonDocument = msgObj.ToBsonDocument();
        // Process
        if (msgObj == null)
        {
            Console.WriteLine("Error the obj not valid");
        }
        await _mongoService.InsertAsync("NorthCommamnd", bsonDocument);

    }
}
