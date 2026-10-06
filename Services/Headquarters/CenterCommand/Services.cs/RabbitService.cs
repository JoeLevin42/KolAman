using CenterCommand.Configuration;
using CenterCommand.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Text.Json;

namespace CenterCommand.Services;

public class Pipeline
{
    private readonly RabbitConfiguration _rabbitConfig;
    

    public Pipeline(RabbitConfiguration rabbitConfig
        )
    {
        _rabbitConfig = rabbitConfig;
        
    }

    public async Task Run()
    {
        var factory = new ConnectionFactory { HostName = _rabbitConfig.Host };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        // now need to declare the regular queue (wiht the argument to dead-letter)

        await channel.QueueDeclareAsync(queue: _rabbitConfig.Queue, durable: true, exclusive: false, autoDelete: false,
        arguments: null);


       

        //now create the actual consuem
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                //need to desirlize the message to the object and do with this somethign
                var obj = JsonSerializer.Deserialize<Alert>(message);
                if (obj == null)
                {
                    throw new Exception();
                }

                //send to db
               
                
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine($" [x] Received {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
        };
        await channel.BasicConsumeAsync(_rabbitConfig.Queue, autoAck: false, consumer: consumer);
        await Task.Delay(Timeout.Infinite);
    }
}