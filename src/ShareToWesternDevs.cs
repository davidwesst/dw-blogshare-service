using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DW.Website.BlogShare
{
    public class ShareToWesternDevs
    {
        private readonly ILogger<ShareToWesternDevs> _logger;

        public ShareToWesternDevs(ILogger<ShareToWesternDevs> logger)
        {
            _logger = logger;
        }

        [Function("ShareToWesternDevs")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
