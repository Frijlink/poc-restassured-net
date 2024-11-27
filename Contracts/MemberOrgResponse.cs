using Newtonsoft.Json;

namespace PocRestAssuredNet.Contracts;

public class MemberOrgResponse
{
    [JsonProperty("id")]
    public required string OrganizationId { get; set; }

    [JsonProperty("idMemberCreator")]
    public required string MemberId { get; set; }
}