using Newtonsoft.Json;

namespace PocRestAssuredNet.Contracts;

public class BoardsResponse
{
    [JsonProperty("id")]
    public required string BoardId { get; set; }
}