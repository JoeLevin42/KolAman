


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OperationsRoom.Configuraion;
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

services.AddSingleton(mongoConfig);
services.AddSingleton<ProcessorService>();
services.AddSingleton<Pipeline>();

var provider = services.BuildServiceProvider();

var pipeline = provider.GetRequiredService<Pipeline>();

await pipeline.Run();
