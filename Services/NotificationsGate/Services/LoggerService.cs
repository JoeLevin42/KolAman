using NotificationsGate.Models;

namespace NotificationsGate.Services;


public class LoggerService
{
    private readonly ElasticService _elastic;

    public LoggerService(ElasticService elastic)
    {
        _elastic = elastic;
    }

    public async Task Log(string level , Log log)
    {

    }
}