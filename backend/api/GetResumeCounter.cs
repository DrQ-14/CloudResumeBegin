using System.Configuration;
using System.Reflection.Metadata;
using Grpc.Core;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;

namespace api;

public class GetResumeCounter
{
    //private readonly ILogger<GetResumeCounter> _logger;
    
    //[Function("GetResumeCounter")]
    //CosmosDBOutput binding to write data to CosmosDB
    [CosmosDBOutput(
        databaseName: "AzureResume",
        containerName: "cloud-resume-for-me",
        Connection = "CosmosDBConnection")]
    public async Task<HttpResponseData> Run(
        //Http Trigger
        [HttpTrigger(AuthorizationLevel.Function, "get","post")] HttpRequestData req,

        //CosmosDB input binding
        [CosmosDBInput(
            databaseName: "AzureResume",
            containerName: "cloud-resume-for-me",
            Connection = "CosmosDBConnection",
            Id="{id}",
            PartitionKey = "{id}")]
        IEnumerable<Counter> inputItems)

    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("The api has been properly returned");
        return response;
    }
}