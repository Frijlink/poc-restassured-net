namespace PocRestAssuredNet.Tests;

[SetUpFixture]
public class OneTimeSetup
{
    [OneTimeSetUp]
    public void FirstInit()
    {
        BaseUrl = GetEnvironmentVariable("TRELLO_API_URL");
        Key = GetEnvironmentVariable("TRELLO_API_KEY");
        Token = GetEnvironmentVariable("TRELLO_API_TOKEN");
    }
}