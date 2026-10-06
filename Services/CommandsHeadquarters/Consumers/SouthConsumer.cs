using CommandsHeadquarters.Services;
using RabbitMQ.Client;

namespace CommandsHeadquarters.Consuemrs;

public class SouthConsumer : BaseConsumer
{
    private readonly SouthService _service;

    public SouthConsumer(
        IConnection connection,
        SouthService service)
        : base(connection)
    {
        _service = service;
    }

    protected override async Task HandleMessageAsync(string message)
    {
        await _service.ProcessAsync(message);
    }
}
