using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace api;

public class GetResumeCounter
{
private readonly ILogger<GetResumeCounter> _logger;
    public GetResumeCounter(ILogger<GetResumeCounter> logger)
    {
        _logger = logger;
    }

    [CosmosDBOutput(
databaseName: "AzureResume",
containerName: "Counter",
Connection = "CosmosDBConnection")]
    public CosmosDBCounter OutputItem { get; set; } = default!;
    //private readonly ILogger<GetResumeCounter> _logger;

    [Function("GetResumeCounter")]
    //CosmosDBOutput binding to write data to CosmosDB
    public HttpResponseData Run(
        //Http Trigger
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "counter/pageView")] HttpRequestData req,
        //string id,
        //CosmosDB input binding
        [CosmosDBInput(
            databaseName: "AzureResume",
            containerName: "Counter",
            Connection = "CosmosDBConnection",
            Id="pageView",
            PartitionKey = "id")]
        IEnumerable<CosmosDBCounter> inputItems)

    {
        var counter = inputItems?.FirstOrDefault();
        var response = req.CreateResponse();

        _logger.LogInformation("Function started");

        // Example: logging the counter
        _logger.LogInformation("Counter object: {Counter}", JsonSerializer.Serialize(counter));

        if (counter == null)
        {
            //outputItem = null; // no write
            response.StatusCode = HttpStatusCode.NotFound;
            response.WriteString("CosmosDB item not found.\n");

            var itemCount = inputItems == null ? 0 : inputItems.Count();
            response.WriteString($"InputItems count: {itemCount}\n");

            //var routeId = req.Url.Segments.Last().TrimEnd('/');
            response.WriteString($"Request URL: {req.Url}\n");
            response.WriteString($"Route id (last segment): pageView\n");

            response.WriteString($"Creating new CosmosDB entry:\n");

            counter = new CosmosDBCounter
            {
                id = "pageView",
                Count = 1
            };

            OutputItem = counter;
            response.WriteString($"Current counter value: {counter.Count}\n");
            response.WriteString($"The current counter Id is: {counter.id}\n");
        }
        else
        {
            counter.Count += 1;
            OutputItem = counter;
            response.StatusCode = HttpStatusCode.OK;
            response.WriteString($"Current counter value: {counter.Count}\n");
            response.WriteString($"The current counter Id is: {counter.id}\n");
            response.WriteString("Cosmos DB item retrieved successfully.\n");
        }
        //response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}