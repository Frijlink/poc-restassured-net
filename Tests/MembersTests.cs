using API;
using Contracts;
using FluentAssertions;

namespace Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MembersTests
{
    [Test, Category("@2")]
    public void RetrieveAmountOfBoardsFromMember()
    {
        var key = GetEnvironmentVariable("TRELLO_API_KEY");
        var token = GetEnvironmentVariable("TRELLO_API_TOKEN");

        var boards = MembersApi.GetBoards(key, token);

        boards.Should().BeEquivalentTo(new List<BoardsResponse> {});
    }
}