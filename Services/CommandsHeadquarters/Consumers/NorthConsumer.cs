using CommandsHeadquarters.Services;
using RabbitMQ.Client;

namespace CommandsHeadquarters.Consuemrs;
public class NorthConsumer : BaseConsumer
{
    private readonly NorthService _service;

    public NorthConsumer(
        IConnection connection,
        NorthService service)
        : base(connection)
    {
        _service = service;
    }

    protected override async Task HandleMessageAsync(string message)
    {
        await _service.ProcessAsync(message);
    }
}
