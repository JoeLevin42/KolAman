using Elastic.Clients.Elasticsearch;
using NotificationsGate.Models;
using System.Text.Json.Serialization;


namespace NotificationsGate.Services;


public class LoggerService
{
    private readonly ElasticService _elastic;
    const string IndexName = "logs";
    public LoggerService(ElasticService elastic)
    {
        _elastic = elastic;
    }
    



  
   
    public async Task LogAsync(string level, string message)
    {
        var log = new Log
        {
            Level = level,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        await _elastic.AddToIndexAsync(log);

    }
}



