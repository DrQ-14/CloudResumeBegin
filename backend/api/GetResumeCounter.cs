using System.Configuration;
using System.Reflection.Metadata;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;

namespace Company.Function;

public class GetResumeCounter
{
    private readonly ILogger<GetResumeCounter> _logger; 

    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get","post")] HttpRequestData req,

        //CosmosDB input binding 
        [CosmosDBInput(
            databaseName: "Counter",
            containerName: "AzureResume",
            Connection = "CosmosDBConnection",
            Id="{id}",
            PartitionKey = "{/id}")]
        IEnumerable<Counter> inputItems)
        
        //CosmosDB output binding
        /*[CosmosDBTrigger(
            databaseName: "Counter", 
            containerName: "AzureResume",
            Connection = "cloud-resume-for-me")]
            IAsyncCollector<Counter> outputItem
        )*/
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(inputItems);
        return response;
    }
}