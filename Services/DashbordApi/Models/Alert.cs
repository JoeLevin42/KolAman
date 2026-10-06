using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace OperationsRoom.Models;


public class Alert
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("alert_id")]
    [JsonPropertyName("alert_id")]
    public string AlertId { get; set; }

    [BsonElement("source")]
    [JsonPropertyName("source")]
    public string Source { get; set; }

    [BsonElement("title")]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [BsonElement("content")]
    [JsonPropertyName("content")]
    public string Content { get; set; }

    [BsonElement("priority")]
    [JsonPropertyName("priority")]
    public string Priority { get; set; }

    [BsonElement("classification")]
    [JsonPropertyName("classification")]
    public string Classification { get; set; }

    [BsonElement("lat")]
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [BsonElement("lon")]
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [BsonElement("timestamp")]
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    [BsonElement("status")]
    [JsonPropertyName("status")]
    public string Status { get; set; }



    [BsonElement("command_name")]
    [JsonPropertyName("command_name")]
    public string CommandName { get; set; }


}
