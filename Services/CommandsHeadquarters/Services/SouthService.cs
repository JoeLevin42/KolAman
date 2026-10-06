
using CommandsHeadquarters.Models;
using MongoDB.Bson;
using System.Text.Json;

namespace CommandsHeadquarters.Services;

public class SouthService
{
    private readonly MongoService _mongoService;
    private readonly LoggerService _logger;

    public SouthService(MongoService mongoService,
        LoggerService logger)
    {
        _mongoService = mongoService;
        _logger = logger;
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
        await _mongoService.InsertAsync("SouthCommand", bsonDocument);
        await _logger.LogAsync("INFO", $"sent to SouthCommand{message}");
    }
}
