
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using OperationsRoom.Configuration;
using OperationsRoom.Models;


namespace OperationsRoom.Services;

public class ElasticService
{

    private readonly ElasticConfiguration _config;
    private readonly ElasticsearchClient _client;
    const string IndexNameIndications = "indications";
    public ElasticService(ElasticConfiguration config)
    {
        _config = config;

        var settings = new ElasticsearchClientSettings(new Uri(_config.Url));
        _client = new ElasticsearchClient(settings);
    }

    public async Task CreateIndexAsync()
    {
        var exists = await _client.Indices.ExistsAsync(IndexNameIndications);

        if (exists.Exists)
        {
            return;
        }

        var response = await _client.Indices.CreateAsync(IndexNameIndications);

        if (!response.IsValidResponse)
        {
            Console.WriteLine("Failed to create logs index");
            Console.WriteLine(response.ToString());
     
        }
    }

    public async Task AddToIndexAsync(Log log) // need to be the log
    {
        var response = await _client.IndexAsync(log, x => x.Index(_config.IndexName));

        if (!response.IsValidResponse)
        {
            Console.WriteLine("Failed to write log to Elasticsearch.");
        }
    }

    public async Task SendToIndicationAsync(int IndicationCount)
    {
        var indicationDoc = new IndicationDoc
        {
            Timestamp = DateTime.UtcNow,
            CommonAlerts = IndicationCount
        };

        var response = await _client.IndexAsync(indicationDoc, x => x.Index(IndexNameIndications));

        if (!response.IsValidResponse)
        {
            Console.WriteLine("Failed to write log to Elasticsearch.");
        }
    }



}