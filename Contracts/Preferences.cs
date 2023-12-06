using Newtonsoft.Json;

namespace Contracts;

public class Preferences
{
    [JsonProperty("background")]
    public required string Background { get; set; }

    [JsonProperty("permissionLevel")]
    public required string PermissionLevel { get; set; }
}
