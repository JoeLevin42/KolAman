using DashbordApi.Services;
using Microsoft.AspNetCore.Mvc;
using OperationsRoom.Models.Dto;

namespace DashbordApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DashbordController : ControllerBase
{
    private readonly MongoService _mongoService;
    public DashbordController(MongoService mongoService)
    {
        _mongoService = mongoService;
    }
    [HttpGet("count_alerts")]
    public async Task<ActionResult<CommandsAlertCount>> GetCommandAlertCountAsync()
    {
        var res = await _mongoService.GetAllCommandCount();
        return Ok(res);
    }
    [HttpGet("alert_by_priority")]
    public async Task<ActionResult<object>> GetSegmentationByPriorityAsync()
    {
        var resultObj = await _mongoService.SegmentationByPriorityAsync();
        return Ok(resultObj);
    }
    [HttpGet("alert_by_status")]
    public async Task<ActionResult<object>> SegmentationByStatusAsync()
    {
        var resultObj = await _mongoService.SegmentationByStatusAsync();
        return Ok(resultObj);
    }
}