using CommandsHeadquarters.Services;
using RabbitMQ.Client;

namespace CommandsHeadquarters.Consuemrs;

public class CenterConsumer : BaseConsumer
{
    private readonly CenterService _service;

    public CenterConsumer(
        IConnection connection,
        CenterService service)
        : base(connection)
    {
        _service = service;
    }

    protected override async Task HandleMessageAsync(string message)
    {
        await _service.ProcessAsync(message);
    }
}
