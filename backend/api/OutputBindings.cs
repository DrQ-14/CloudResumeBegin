using api;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class Outputbindings
{
    [CosmosDBOutput(
        databaseName: "AzureResume",
        containerName: "cloud-resume-for-me",
        Connection = "CosmosDBConnection")]
        public CosmosDBCounter OutputItem { get; set; }

        public HttpResponseData HttpResponse { get; set; }
}