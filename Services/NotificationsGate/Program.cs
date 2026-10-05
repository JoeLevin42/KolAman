
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
services.AddSingleton<FileLoaderService>();

var serviceProvider = services.BuildServiceProvider();


var kafkaService = serviceProvider.GetRequiredService<KafkaService>();
var fileLoader = serviceProvider.GetRequiredService<FileLoaderService>();


using var watcher = new FileSystemWatcher("C:\\Users\\JL202\\Desktop\\KolAman\\alert-simulator\\alerts");

watcher.NotifyFilter = NotifyFilters.Attributes
                     | NotifyFilters.CreationTime
                     | NotifyFilters.DirectoryName
                     | NotifyFilters.FileName
                     | NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.Security
                     | NotifyFilters.Size;


watcher.Created += OnCreated;

watcher.Filter = "*.ready";
watcher.IncludeSubdirectories = true;
watcher.EnableRaisingEvents = true;


Console.WriteLine("Press enter to exit.");
Console.ReadLine();

static void OnCreated(object sender, FileSystemEventArgs e )
{
    var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"));
    var eclient = new ElasticsearchClient(settings);
    var response = eclient.Indices.CreateAsync("logs");
    string value = e.FullPath;
    string newPath = Path.ChangeExtension(value, ".json");
    IProducer<Null, string> producer;
    var kafkaConfig = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();

    try
    {
        var file = File.ReadAllText(newPath);
        var fileObj = JsonSerializer.Deserialize<Alert>(file);
        var json = JsonSerializer.Serialize(fileObj);

        if (fileObj != null)
        {
            producer.ProduceAsync("alerts", new Message<Null, string>
            {
                Value = json
            });
        }

    }

    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}



