using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace api;

public class GetResumeCounter
{
    [CosmosDBOutput(
databaseName: "AzureResume",
containerName: "cloud-resume-for-me",
Connection = "CosmosDBConnection")]
    public CosmosDBCounter outputItem { get; set; } = default!;
    //private readonly ILogger<GetResumeCounter> _logger;

    [Function("GetResumeCounter")]
    //CosmosDBOutput binding to write data to CosmosDB
    public HttpResponseData Run(
        //Http Trigger
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "counter/{id}")] HttpRequestData req,
        //CosmosDB input binding
        [CosmosDBInput(
            databaseName: "AzureResume",
            containerName: "cloud-resume-for-me",
            Connection = "CosmosDBConnection",
            Id="{id}",
            PartitionKey = "{id}")]
        IEnumerable<CosmosDBCounter> inputItems)

    {
        var counter = inputItems?.FirstOrDefault();
        var response = req.CreateResponse();

        if (counter == null)
        {
            //outputItem = null; // no write
            response.StatusCode = HttpStatusCode.NotFound;
            response.WriteString("CosmosDB item not found.\n");
            response.WriteString($"InputItems count: {inputItems?.Count() ?? 0}\n");
            
            var routeId = req.Url.Segments.Last().TrimEnd('/');
            response.WriteString($"Request URL: {req.Url}\n");
            response.WriteString($"Route id (last segment): {routeId}\n");
        }
        else
        {
            counter.Count += 1;
            outputItem = counter;
            response.StatusCode = HttpStatusCode.OK;
            response.WriteString($"Current counter value: {counter.Count}\n");
            response.WriteString($"The current counter Id is: {counter.id}\n");
            response.WriteString("DEBUG: Cosmos DB item retrieved successfully.\n");
        }
        //response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}