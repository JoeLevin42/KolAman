using Confluent.Kafka;
using NotificationsGate.Configuration;
using NotificationsGate.Models;
using System.Text.Json;

namespace NotificationsGate.Services;


public class KafkaService
{
    private readonly KafkaConfiguration _config;

    private readonly IProducer<Null, string> _producer;
    public KafkaService(KafkaConfiguration config)
    {
        _config = config;
        var kafkaConfig = new ProducerConfig
        {
            BootstrapServers = _config.BootstrapServers
        };


        _producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();

    }

    public async Task SendToKafkaAsync(Alert alert)
    {
        try
        {
            var jsonAlert = JsonSerializer.Serialize(alert);

            await _producer.ProduceAsync(_config.Topic, new Message<Null, string>
            {
                Value = jsonAlert
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
    
