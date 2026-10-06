using System.Text.Json.Serialization;

namespace OperationsRoom.Models;


public class Log
{
    [JsonPropertyName("level")]
    public string Level { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}