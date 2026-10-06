using CommandsHeadquarters.Services;
using RabbitMQ.Client;

namespace CommandsHeadquarters.Consuemrs;

public class OverseasConsumer : BaseConsumer
{
    private readonly OverseasService _service;

    public OverseasConsumer(
        IConnection connection,
        OverseasService service)
        : base(connection)
    {
        _service = service;
    }

    protected override async Task HandleMessageAsync(string message)
    {
        await _service.ProcessAsync(message);
    }
}
