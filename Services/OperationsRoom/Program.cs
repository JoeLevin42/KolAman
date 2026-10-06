


using CommandsHeadquarters.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OperationsRoom.Configuraion;
using OperationsRoom.Configuration;
using OperationsRoom.Pipeline;
using OperationsRoom.Services;

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();
//register all here

var mongoConfig = new MongoConfiguration
{
    ConnectionString = configuration["Mongo:ConnectionString"]!,
    DatabaseName = configuration["Mongo:DatabaseName"]!
};

var elasticConfig = new ElasticConfiguration
{
    Url = configuration["Elastic:Url"]!,
    IndexName = configuration["Elastic:IndexName"]!
};

services.AddSingleton(elasticConfig);
services.AddSingleton<ElasticService>();
services.AddSingleton<LoggerService>();
services.AddSingleton(mongoConfig);
services.AddSingleton<ProcessorService>();
services.AddSingleton<Pipeline>();

var provider = services.BuildServiceProvider();

//create elastic indications index
var elasticService = provider.GetRequiredService<ElasticService>();
await elasticService.CreateIndexAsync();

var pipeline = provider.GetRequiredService<Pipeline>();

await pipeline.Run();
