using Contracts;

namespace API;

public class MembersApi
{
    public static List<BoardsResponse> GetBoards(string key, string token)
    {
        return (List<BoardsResponse>)Given()
            .PathParam("key", key)
            .PathParam("token", token)
            .When()
            .Get($"{BaseUrl}/1/members/me/boards?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(List<BoardsResponse>));
    }

    public static List<MemberOrgResponse> GetMemberOrganizations(string memberId, string key, string token)
    {
        Dictionary<string, object> pathParams = new()
        {
            { "memberId", memberId },
            { "key", key },
            { "token", token },
        };

        return (List<MemberOrgResponse>)Given()
            .PathParams(pathParams)
            .When()
            .Get($"{BaseUrl}/1/members/[memberId]/organizations?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(List<MemberOrgResponse>));
    }
}