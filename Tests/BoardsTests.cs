using Contracts;
using FluentAssertions;
using Namotion.Reflection;
using static Settings.Configuration;

namespace Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class BoardsTests
{
    private string baseUrl = string.Empty;
    private string token = string.Empty;
    private string key = string.Empty;
    private string boardId = string.Empty;
    private string organizationId = string.Empty;

    [SetUp]
    public void Init()
    {
        baseUrl = GetEnvironmentVariable("TRELLO_API_URL");
        token = GetEnvironmentVariable("TRELLO_API_TOKEN");
        key = GetEnvironmentVariable("TRELLO_API_KEY");

        var memberId = GetMemberId(key, token);
        memberId.Should().NotBeNull();

        var memberOrgs = GetMemberOrganizations(memberId, key, token);
        memberOrgs[0].MemberId.Should().Be(memberId);
        memberOrgs[0].OrganizationId.Should().NotBeNullOrEmpty();
        organizationId = memberOrgs[0].OrganizationId;
    }

    [TearDown]
    public void DeleteAllBoards()
    {
        var boards = (List<BoardsResponse>)Given()
            .PathParam("key", key)
            .PathParam("token", token)
            .When()
            .Get($"{baseUrl}/1/members/me/boards?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(List<BoardsResponse>));

        foreach (var board in boards)
        {
            DeleteBoard(board.BoardId, key, token);
        }
    }

    [Test, Category("API")]
    public void CreateAndDeleteTrelloBoard()
    {
        Dictionary<string, string> newBoard = new()
        {
            { "name", TestDataGenerator.GenerateBoardName() },
            { "updatedName", TestDataGenerator.GenerateBoardName() },
            { "colour", "purple" },
            { "updatedcolour", "pink" },
            { "permissionLevel", "org" },
            { "updatedpermissionLevel", "private" },
        };

        // Create Board
        var responseCreate = CreateBoard(key, token, newBoard["name"], newBoard["colour"], newBoard["permissionLevel"]);
        responseCreate.BoardId.Should().NotBeNullOrEmpty();
        responseCreate.OrganizationId.Should().Be(organizationId);
        responseCreate.Name.Should().Be(newBoard["name"]);
        responseCreate.Closed.Should().BeFalse();
        responseCreate.Preferences.Background.Should().Be(newBoard["colour"]);
        responseCreate.Preferences.PermissionLevel.Should().Be(newBoard["permissionLevel"]);
        boardId = responseCreate.BoardId;

        // Read Board
        var responseGet = GetBoard(boardId, key, token);
        responseGet.BoardId.Should().Be(boardId);
        responseGet.OrganizationId.Should().Be(organizationId);
        responseGet.Name.Should().Be(newBoard["name"]);
        responseGet.Closed.Should().BeFalse();
        responseGet.Preferences.Background.Should().Be(newBoard["colour"]);
        responseGet.Preferences.PermissionLevel.Should().Be(newBoard["permissionLevel"]);

        // Update Board
        var responseUpdate = UpdateBoard(boardId, key, token, newBoard["updatedName"], newBoard["updatedcolour"], newBoard["updatedpermissionLevel"]);
        responseUpdate.BoardId.Should().Be(boardId);
        responseUpdate.OrganizationId.Should().Be(organizationId);
        responseUpdate.Name.Should().Be(newBoard["updatedName"]);
        responseUpdate.Closed.Should().BeFalse();
        responseUpdate.Preferences.Background.Should().Be(newBoard["updatedcolour"]);
        responseUpdate.Preferences.PermissionLevel.Should().Be(newBoard["updatedpermissionLevel"]);

        // Close Board
        var responseClose = CloseBoard(boardId, key, token);
        responseClose.BoardId.Should().Be(boardId);
        responseClose.Closed.Should().BeTrue();

        // Delete Board
        DeleteBoard(boardId, key, token);
    }

    public string GetMemberId(string key, string token)
    {
        return (string)Given()
            .PathParam("key", key)
            .PathParam("token", token)
            .When()
            .Get($"{baseUrl}/1/tokens/[token]?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .Extract().Body("$.idMember");
    }

    public List<MemberOrgResponse> GetMemberOrganizations(string memberId, string key, string token)
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
            .Get($"{baseUrl}/1/members/[memberId]/organizations?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(List<MemberOrgResponse>));
    }

    public CreateBoardResponse CreateBoard(
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
            .Post($"{baseUrl}/1/boards")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

    public CreateBoardResponse GetBoard(string boardId, string key, string token)
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
            .Get($"{baseUrl}/1/boards/[boardId]?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

    public CreateBoardResponse UpdateBoard(
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
            .Put($"{baseUrl}/1/boards/{boardId}")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

    public CreateBoardResponse CloseBoard(
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
            .Put($"{baseUrl}/1/boards/{boardId}")
            .Then()
            .StatusCode(200)
            .DeserializeTo(typeof(CreateBoardResponse));
    }

     public void DeleteBoard(string boardId, string key, string token)
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
            .Delete($"{baseUrl}/1/boards/[boardId]?key=[key]&token=[token]")
            .Then()
            .StatusCode(200);
    }
}
