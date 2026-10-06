using DashbordApi.Configuraion;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Bson;
using MongoDB.Driver;
using OperationsRoom.Models;
using OperationsRoom.Models.Dto;

namespace DashbordApi.Services;

public class MongoService
{
    private readonly IMongoDatabase _database;

    public MongoService(MongoConfiguration configuration)
    {
        var client = new MongoClient(configuration.ConnectionString);

        _database = client.GetDatabase(configuration.DatabaseName);
        
    }

    public async Task<CommandsAlertCount> GetAllCommandCount()
    {
        var cnterCollectionCount = _database.GetCollection<BsonDocument>("CenterCommand")
                                .CountDocuments(_=> true);
        var northCollectionCount = _database.GetCollection<BsonDocument>("NorthCommand")
                                .CountDocuments(_ => true);
                                
        var southCollectionCount = _database.GetCollection<BsonDocument>("SouthCommand")
                                .CountDocuments(_ => true);
        var overseasCollectionCount = _database.GetCollection<BsonDocument>("OverseasCommand")
                                .CountDocuments(_ => true);

        var result = new CommandsAlertCount
        {
            CenterCommand = (int)cnterCollectionCount,
            NorthCommand = (int)northCollectionCount,
            SouthCommand = (int)southCollectionCount,
            OverseasCommand = (int)overseasCollectionCount
        };

        return result;
    }

    public async Task<object> SegmentationByPriorityAsync()
    {
        var centerByPriority = _database.GetCollection<Alert>("CenterCommand").AsQueryable()
                             .GroupBy(a => a.Priority)
                            .Select(g => new { Priority = g.Key, Count = g.Count() 
                            }).ToList();

        var northByPriority = _database.GetCollection<Alert>("NorthCommand").AsQueryable()
                             .GroupBy(a => a.Priority)
                            .Select(g => new { Priority = g.Key, Count = g.Count() 
                            }).ToList();
        var southByPriority = _database.GetCollection<Alert>("SouthCommand").AsQueryable()
                             .GroupBy(a => a.Priority)
                            .Select(g => new { Priority = g.Key, Count = g.Count()
                            }).ToList();
        var overseasByPriority = _database.GetCollection<Alert>("OverseasCommand").AsQueryable()
                             .GroupBy(a => a.Priority)
                            .Select(g => new { Priority = g.Key, Count = g.Count() 
                            }).ToList();

        var priorityByCentersObj = new
        {
            CenterCommand = centerByPriority,
            NorthCommand = northByPriority,
            SouthByPriority = southByPriority,
            OverseasByPriority = overseasByPriority
        };
        return priorityByCentersObj;

    }

    public async Task<object> SegmentationByStatusAsync()
    {
        var centerByPriority = _database.GetCollection<Alert>("CenterCommand").AsQueryable()
                             .GroupBy(a => a.Status)
                            .Select(g => new {
                                Priority = g.Key,
                                Count = g.Count()
                            }).ToList();

        var northByPriority = _database.GetCollection<Alert>("NorthCommand").AsQueryable()
                             .GroupBy(a => a.Status)
                            .Select(g => new {
                                Priority = g.Key,
                                Count = g.Count()
                            }).ToList();
        var southByPriority = _database.GetCollection<Alert>("SouthCommand").AsQueryable()
                             .GroupBy(a => a.Status)
                            .Select(g => new {
                                Priority = g.Key,
                                Count = g.Count()
                            }).ToList();
        var overseasByPriority = _database.GetCollection<Alert>("OverseasCommand").AsQueryable()
                             .GroupBy(a => a.Status)
                            .Select(g => new {
                                Priority = g.Key,
                                Count = g.Count()
                            }).ToList();

        var priorityByCentersObj = new
        {
            CenterCommand = centerByPriority,
            NorthCommand = northByPriority,
            SouthByPriority = southByPriority,
            OverseasByPriority = overseasByPriority
        };
        return priorityByCentersObj;

    }






}