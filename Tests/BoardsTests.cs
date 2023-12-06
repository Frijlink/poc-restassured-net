using API;
using FluentAssertions;

namespace Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class BoardsTests
{
    private string boardId = string.Empty;
    private string organizationId = string.Empty;

    [SetUp]
    public void Init()
    {
        var memberId = TokenApi.GetMemberId(Key, Token);
        memberId.Should().NotBeNull();

        var memberOrgs = MembersApi.GetMemberOrganizations(memberId, Key, Token);
        memberOrgs[0].MemberId.Should().Be(memberId);
        memberOrgs[0].OrganizationId.Should().NotBeNullOrEmpty();
        organizationId = memberOrgs[0].OrganizationId;
    }

    [TearDown]
    public void DeleteAllBoards()
    {
        var boards = MembersApi.GetBoards(Key, Token);

        foreach (var board in boards)
        {
            BoardsApi.DeleteBoard(board.BoardId, Key, Token);
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
        var responseCreate = BoardsApi.CreateBoard(Key, Token, newBoard["name"], newBoard["colour"], newBoard["permissionLevel"]);
        responseCreate.BoardId.Should().NotBeNullOrEmpty();
        responseCreate.OrganizationId.Should().Be(organizationId);
        responseCreate.Name.Should().Be(newBoard["name"]);
        responseCreate.Closed.Should().BeFalse();
        responseCreate.Preferences.Background.Should().Be(newBoard["colour"]);
        responseCreate.Preferences.PermissionLevel.Should().Be(newBoard["permissionLevel"]);
        boardId = responseCreate.BoardId;

        // Read Board
        var responseGet = BoardsApi.GetBoard(boardId, Key, Token);
        responseGet.BoardId.Should().Be(boardId);
        responseGet.OrganizationId.Should().Be(organizationId);
        responseGet.Name.Should().Be(newBoard["name"]);
        responseGet.Closed.Should().BeFalse();
        responseGet.Preferences.Background.Should().Be(newBoard["colour"]);
        responseGet.Preferences.PermissionLevel.Should().Be(newBoard["permissionLevel"]);

        // Update Board
        var responseUpdate = BoardsApi.UpdateBoard(boardId, Key, Token, newBoard["updatedName"], newBoard["updatedcolour"], newBoard["updatedpermissionLevel"]);
        responseUpdate.BoardId.Should().Be(boardId);
        responseUpdate.OrganizationId.Should().Be(organizationId);
        responseUpdate.Name.Should().Be(newBoard["updatedName"]);
        responseUpdate.Closed.Should().BeFalse();
        responseUpdate.Preferences.Background.Should().Be(newBoard["updatedcolour"]);
        responseUpdate.Preferences.PermissionLevel.Should().Be(newBoard["updatedpermissionLevel"]);

        // Close Board
        var responseClose = BoardsApi.CloseBoard(boardId, Key, Token);
        responseClose.BoardId.Should().Be(boardId);
        responseClose.Closed.Should().BeTrue();

        // Delete Board
        BoardsApi.DeleteBoard(boardId, Key, Token);
    }
}
