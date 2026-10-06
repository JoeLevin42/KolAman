using CommandsHeadquarters.Services;
using MongoDB.Driver;
using OperationsRoom.Configuraion;
using OperationsRoom.Models;
using System.Collections.Generic;
namespace OperationsRoom.Services;

public class ProcessorService
{

    private readonly MongoConfiguration _mongoConfig;

    private readonly IMongoDatabase _database;
    private readonly LoggerService _logger;
    public ProcessorService(MongoConfiguration mongoConfig,
        LoggerService logger)
    {
        _mongoConfig = mongoConfig;
        var client = new MongoClient(_mongoConfig.ConnectionString);

        _database = client.GetDatabase(_mongoConfig.DatabaseName);
        _logger = logger;
    }

    public async Task<IList<string>> ProccessCollection(string collectionName)
    {
      
        Console.WriteLine($"Processor Service started {collectionName}");

        var collection = _database.GetCollection<Alert>(collectionName);

        List<string> titles = new List<string>();
        var items = await collection.Find(x => x.Status == "WAITING").ToListAsync();
        foreach (var item in items)
        {
            try {
                // save the title object in list
                titles.Add(item.Title);
                item.Status = "INPROGRESS";
                await collection.ReplaceOneAsync(x => x.Id == item.Id, item); // changed the status before process
                // time by prior
                var delayTime = 3;

                Console.WriteLine($"Start working on item {item.Id}");
                if (item.Priority == "CRITICAL") { delayTime = 4; }
                if (item.Priority == "HIGH") { delayTime = 3; }
                if (item.Priority == "MEDIUM ") { delayTime = 2; }
                if (item.Priority == "LOW ")
                {
                    item.Status = "CANCEl";
                    await collection.ReplaceOneAsync(x => x.Id == item.Id, item);
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(delayTime));
                item.Status = "DONE";
                await collection.ReplaceOneAsync(x => x.Id == item.Id, item);
                await _logger.LogAsync("INFO", $"Processed the Alert took {delayTime} seconds");
                Console.WriteLine($"Finisehd work on item {item.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            }
            
            return titles; // list of all the titles of this cycle
            }
        

}


// all title of this cycle here
//foreach (var title in titles)
//{
//    Console.WriteLine(title);
//}
//}