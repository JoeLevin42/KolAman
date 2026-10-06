

using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using NotificationsGate.Configuration;
using NotificationsGate.Models;

namespace NotificationsGate.Services;

public class ElasticService
{

    private readonly ElasticConfiguration _config;
    private readonly ElasticsearchClient _client;

    public ElasticService(ElasticConfiguration config)
    {
        _config = config;

        var settings = new ElasticsearchClientSettings(new Uri(_config.Url));
        _client = new ElasticsearchClient(settings);
    }

    public async Task CreateIndexAsync()
    {
        var exists = await _client.Indices.ExistsAsync(_config.IndexName);

        if (exists.Exists)
        {
            return;
        }

        var response = await _client.Indices.CreateAsync("logs");

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
}
