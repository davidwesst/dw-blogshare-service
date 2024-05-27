using System.Text.Json;
using DW.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DW.Website
{
    public class BlogShare
    {
        private readonly ILogger<BlogShare> _logger;

        public BlogShare(ILogger<BlogShare> logger)
        {
            _logger = logger;
        }

        [Function("blogshare")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "blogshare/{destination?}")]
            HttpRequest req, 
            string destination
            )
        {
            try
            {
                var blogPost = JsonSerializer.DeserializeAsync<BlogShare>(req.Body);

                switch (destination)
                {
                    case "wd":
                        _logger.LogInformation("Request to share to WesternDevs received.");
                        break;
                    default:
                        return new BadRequestObjectResult("Include destination parameter in path (e.g. /blogshare/wd)");
                }

                _logger.LogInformation("C# HTTP trigger function processed a request.");
                return new OkObjectResult("Welcome to Azure Functions!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BadRequestObjectResult("Error!");
            }
        }
    }
}
