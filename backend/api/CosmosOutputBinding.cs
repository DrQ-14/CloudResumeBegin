using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;

namespace Company.Function;

public class CosmosOutputBinding
{
    public HttpResponseData HttpResponse { get; set; }

    [CosmosDBOutput(
        databaseName: "Counter",
        containerName: "AzureResume",
        Connection = "AzureResumeConnectionString"
    )]
    public Counter outputCounter { get; set; }
}