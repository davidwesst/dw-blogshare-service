using System.Text.Json;
using DW.Website.Models;
using DW.Website.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace DW.Website
{
    public class Crosspost 
    {
        private readonly ILogger<Crosspost> _logger;

        public Crosspost(ILogger<Crosspost> logger)
        {
            _logger = logger;
        }

        [OpenApiOperation(operationId: "crosspost", tags: new[] { "crosspost" }, Summary = "Crosspost Blog", Description = "Crosspost a normalized blog post to the specified destination", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BlogPost))]
        [Function("crosspost")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "crosspost/{destination?}")]
            HttpRequest req, 
            string destination
            )
        {
            try
            {
                var blogPost = await JsonSerializer.DeserializeAsync<BlogPost>(req.Body);

                if (blogPost != null)
                {
                    switch (destination)
                    {
                        case "wd":
                            _logger.LogInformation("Request to crosspost to WesternDevs received.");

                            // normalize post
                            var normalizeService = new NormalizeService(_logger);
                            var normalizedPost = normalizeService.Normalize(blogPost);

                            // execute crosspost

                            break;
                        default:
                            return new BadRequestObjectResult("Include destination parameter in path (e.g. /blogshare/wd)");
                    }

                    _logger.LogInformation("C# HTTP trigger function processed a request.");
                    return new OkObjectResult("Welcome to Azure Functions!");
                }
                else
                {
                    return new BadRequestObjectResult("The variable blogPost was null.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BadRequestObjectResult("Error!");
            }
        }

    }
}
