using CommandsHeadquarters.Configuraion;
using CommandsHeadquarters.Configuration;
using CommandsHeadquarters.Consuemrs;
using CommandsHeadquarters.Models;
using CommandsHeadquarters.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
    

var services = new ServiceCollection();

services.AddSingleton(configuration);

//services.AddSingleton<RabbitConfiguration>(sp =>
//{
//    var config = sp.GetRequiredService<IConfiguration>();

//    return new RabbitConfiguration
//    {
//        HostName = config["Rabbit:HostName"] ?? "localhost"
//    };
//});

var elasticConfig = new ElasticConfiguration
{
    Url = configuration["Elastic:Url"]!,
    IndexName = configuration["Elastic:IndexName"]!
};

services.AddSingleton(elasticConfig);
services.AddSingleton<ElasticService>();
services.AddSingleton<LoggerService>();

var rabbitConfig = new RabbitConfiguration
{
    HostName = configuration["Rabbit:HostName"] ?? "localhost"
};

var factory = new ConnectionFactory
{
    HostName = rabbitConfig.HostName
};


var connection = await factory.CreateConnectionAsync();

services.AddSingleton<IConnection>(connection);

//Mongo
var mongoConfig = new MongoConfiguration
{
   ConnectionString = configuration["Mongo:ConnectionString"]!,
   DatabaseName = configuration["Mongo:DatabaseName"]!
};

services.AddSingleton(mongoConfig);
services.AddSingleton<MongoService>();

//Commands Services

services.AddSingleton<NorthService>();
services.AddSingleton<SouthService>();
services.AddSingleton<CenterService>();
services.AddSingleton<OverseasService>();

services.AddSingleton<NorthConsumer>();
services.AddSingleton<SouthConsumer>();
services.AddSingleton<CenterConsumer>();
services.AddSingleton<OverseasConsumer>();

var provider = services.BuildServiceProvider();

var north = provider.GetRequiredService<NorthConsumer>();
var south = provider.GetRequiredService<SouthConsumer>();
var center = provider.GetRequiredService<CenterConsumer>();
var overseas = provider.GetRequiredService<OverseasConsumer>();

await north.StartAsync("alerts.north");
await south.StartAsync("alerts.south");
await center.StartAsync("alerts.center");
await overseas.StartAsync("alerts.overseas");

Console.WriteLine("All consumers are running.");

await Task.Delay(Timeout.Infinite);
