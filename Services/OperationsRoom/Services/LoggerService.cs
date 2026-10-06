
using Elastic.Clients.Elasticsearch;
using OperationsRoom.Models;
using OperationsRoom.Services;
using System.Text.Json.Serialization;


namespace CommandsHeadquarters.Services;


public class LoggerService
{
    private readonly ElasticService _elastic;
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



