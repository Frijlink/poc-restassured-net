namespace API;

public class TokenApi
{
    public static string GetMemberId(string key, string token)
    {
        return (string)Given()
            .PathParam("key", key)
            .PathParam("token", token)
            .When()
            .Get($"{BaseUrl}/1/tokens/[token]?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .Extract().Body("$.idMember");
    }
}