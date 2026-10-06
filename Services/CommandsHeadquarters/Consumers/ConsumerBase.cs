using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public abstract class BaseConsumer
{
    private readonly IConnection _connection;

    protected BaseConsumer(IConnection connection)
    {
        _connection = connection;
    }

    public async Task StartAsync(string queueName)
    {
        var channel = await _connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                await HandleMessageAsync(message);

                await channel.BasicAckAsync(
                    ea.DeliveryTag,
                    multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                await channel.BasicNackAsync(
                    ea.DeliveryTag,
                    multiple: false,
                    requeue: false);
            }
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);

        Console.WriteLine($"{queueName} consumer started");
    }

    protected abstract Task HandleMessageAsync(string message);
}
