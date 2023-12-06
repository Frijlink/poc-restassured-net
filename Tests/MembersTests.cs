using API;
using Contracts;
using FluentAssertions;

namespace Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MembersTests
{
    [Test, Category("API")]
    public void RetrieveAmountOfBoardsFromMember()
    {
        var boards = MembersApi.GetBoards(Key, Token);

        boards.Should().BeEquivalentTo(new List<BoardsResponse> {});
    }
}