using Contracts;

namespace API;

public class BoardsApi
{
    public static CreateBoardResponse CreateBoard(
        string key,
        string token,
        string name,
        string colour,
        string visibility
    )
    {
        var data = new Dictionary<string, object>() {
            { "name", name },
            { "key", key },
            { "token", token },
            { "prefs_background", colour },
            { "prefs_permissionLevel", visibility }
        };

        return (CreateBoardResponse)Given()
            .Body(data)
            .When()
            .Post($"{GetEnvironmentVariable("TRELLO_API_URL")}/1/boards")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

    public static CreateBoardResponse GetBoard(string boardId, string key, string token)
    {
        Dictionary<string, object> pathParams = new()
        {
            { "boardId", boardId },
            { "key", key },
            { "token", token },
        };

        return (CreateBoardResponse)Given()
            .PathParams(pathParams)
            .When()
            .Get($"{GetEnvironmentVariable("TRELLO_API_URL")}/1/boards/[boardId]?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

    public static CreateBoardResponse UpdateBoard(
        string boardId,
        string key,
        string token,
        string name,
        string colour,
        string visibility
    )
    {
        var data = new Dictionary<string, object>() {
            { "name", name },
            { "key", key },
            { "token", token },
            { "prefs/background", colour },
            { "prefs/permissionLevel", visibility }
        };

        return (CreateBoardResponse)Given()
            .Body(data)
            .When()
            .Put($"{GetEnvironmentVariable("TRELLO_API_URL")}/1/boards/{boardId}")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

    public static CreateBoardResponse CloseBoard(
        string boardId,
        string key,
        string token
    )
    {
        var data = new Dictionary<string, object>() {
            { "key", key },
            { "token", token },
            { "closed", true },
        };

        return (CreateBoardResponse)Given()
            .Body(data)
            .When()
            .Put($"{GetEnvironmentVariable("TRELLO_API_URL")}/1/boards/{boardId}")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

     public static void DeleteBoard(string boardId, string key, string token)
    {
        Dictionary<string, object> pathParams = new()
        {
            { "boardId", boardId },
            { "key", key },
            { "token", token },
        };

        Given()
            .PathParams(pathParams)
            .When()
            .Delete($"{GetEnvironmentVariable("TRELLO_API_URL")}/1/boards/[boardId]?key=[key]&token=[token]")
            .Then()
            .StatusCode(200);
    }
}
