using Newtonsoft.Json;

namespace Contracts;

public class CreateBoardResponse
{
    [JsonProperty("id")]
    public required string BoardId { get; set; }

    [JsonProperty("idOrganization")]
    public required string OrganizationId { get; set; }

    [JsonProperty("prefs")]
    public required Preferences Preferences { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("closed")]
    public required bool Closed { get; set; }
}