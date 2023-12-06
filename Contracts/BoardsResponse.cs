using Newtonsoft.Json;

namespace Contracts;

public class BoardsResponse
{
    [JsonProperty("id")]
    public required string BoardId { get; set; }
}