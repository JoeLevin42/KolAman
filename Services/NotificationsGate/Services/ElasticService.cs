

using Elastic.Clients.Elasticsearch;
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
        await _client.Indices.CreateAsync(_config.IndexName);
    }

    public async Task AddToIndexAsync(Log log) // need to be the log
    {
        var response = await _client.IndexAsync(log, x => x.Index(_config.IndexName));
    }
}
