using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class GetResumeCounter
{
    private readonly ILogger<GetResumeCounter> _logger; 

    public static Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get","post", Route = null)] HttpRequest req,
        ILogger log)
    {
        
    }
}