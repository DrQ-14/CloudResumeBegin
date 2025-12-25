using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class VisitorCounter
{
    private readonly ILogger<VisitorCounter> _logger;

    public VisitorCounter(ILogger<VisitorCounter> logger)
    {
        _logger = logger;
    }

    [Function("VisitorCounter")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}