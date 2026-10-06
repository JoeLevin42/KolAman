using OperationsRoom.Models;
using OperationsRoom.Services;
using System.Collections.Generic;
using System.Linq;
namespace OperationsRoom.Pipeline;


public class Pipeline
{
    private readonly ProcessorService _processService;

    private readonly ElasticService _elastic;
    public Pipeline(ProcessorService processService,
         ElasticService elastic)
    {
        _processService = processService;
        _elastic = elastic;
    }


    //every func is just proceess service
    public async Task Run()
    {
        while (true)
        {
            //mybe her is the general all four commands list
            // this is cycle lifetime
            Console.WriteLine("Start to work....");                                           
            IEnumerable<string> CenterCommandTitles = await _processService.ProccessCollection("CenterCommand");
            IEnumerable<string> NorthCommandTitles = await _processService.ProccessCollection("NorthCommand");
            IEnumerable<string> OverseasCommandTitles = await _processService.ProccessCollection("OverseasCommand");
            IEnumerable<string> SouthCommandTitles = await _processService.ProccessCollection("SouthCommand");


            // common values across all three lists
            //if service end with common alerts this is Indication
            var resIndications = CenterCommandTitles
                .Intersect(NorthCommandTitles)
                .Intersect(OverseasCommandTitles)
                .Intersect(SouthCommandTitles);


            await _elastic.SendToIndicationAsync(resIndications.Count());
           
            Console.WriteLine(resIndications.Count());
            Console.WriteLine("resIndications:");
            await Task.Delay(TimeSpan.FromSeconds(5)); //second cycle time
        }
    } 





}
