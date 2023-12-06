using FluentAssertions;
using static Settings.Configuration;

namespace Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MembersTests
{
    [Test, Category("API")]
    public void RetrieveAmountOfBoardsFromMember()
    {
        var baseUrl = GetEnvironmentVariable("TRELLO_API_URL");
        var key = GetEnvironmentVariable("TRELLO_API_KEY");
        var token = GetEnvironmentVariable("TRELLO_API_TOKEN");

        Given()
            .PathParam("key", key)
            .PathParam("token", token)
            .When()
            .Get($"{baseUrl}/1/members/me/boards?key=[key]&token=[token]")
            .Then()
            .StatusCode(200)
            .Extract().Body()
            .Should()
            .Be("[]");
    }
}