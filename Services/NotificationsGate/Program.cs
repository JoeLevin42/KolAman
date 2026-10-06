
//create cofnig object and DI


using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationsGate.Configuration;
using NotificationsGate.Models;
using NotificationsGate.Services;
using System.Text.Json;
using YourProject.Services;
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false, true).Build();


var services = new ServiceCollection();

var kafkaConfig = new KafkaConfiguration
{
    BootstrapServers = configuration["Kafka:BootstrapServers"]!,
    Topic = configuration["Kafka:Topic"]!
};

services.AddSingleton(kafkaConfig);
var elasticConfig = new ElasticConfiguration
{
    Url = configuration["Elastic:Url"]!,
    IndexName = configuration["Elastic:IndexName"]!
};
services.AddSingleton(elasticConfig);
services.AddSingleton<ElasticService>();
services.AddSingleton<LoggerService>();
services.AddSingleton<KafkaService>();
services.AddSingleton<FileWatcherService>();
var serviceProvider = services.BuildServiceProvider();




// Create the logs index before starting the pipeline
var elasticService = serviceProvider.GetRequiredService<ElasticService>();

await elasticService.CreateIndexAsync();

// Start the FileSystemWatcher
var watcher = serviceProvider.GetRequiredService<FileWatcherService>();

Console.WriteLine("File watcher is running.");
Console.WriteLine("Press Enter to stop.");

Console.ReadLine();









