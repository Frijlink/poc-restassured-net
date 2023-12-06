# Proof of Concept RestAssured.Net

Let's test [RestAssured.Net](https://github.com/basdijkstra/rest-assured-net)

## Environment Variables ##

| key              | value                   |
|------------------|-------------------------|
| TRELLO_API_URL   |"https://api.trello.com" |
| TRELLO_API_KEY   |                         |
| TRELLO_API_TOKEN |                         |

## How to run the tests ##

* Run all the tests with `dotnet test`
* Run a single test with `dotnet test --filter "MyClassName"`
* Run a single category with `dotnet test --filter TestCategory=MyCategory`