using System.Text.Json;
using System.Text.Json.Serialization;
using DW.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DW.Website
{
    public class NormalizeBlogPost
    {
        private readonly ILogger<BlogShare> _logger;

        public NormalizeBlogPost(ILogger<BlogShare> logger)
        {
            _logger = logger;
        }

        [Function("normalize")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "normalize")]
            HttpRequest req, 
            string destination
            )
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var blogPostBody = JsonSerializer.Deserialize<BlogPost>(requestBody);

                _logger.LogInformation("Processed Normalize Request.");

                return new OkObjectResult(blogPostBody);
            }
            catch (JsonException ex)
            {
                _logger.LogError("A JsonException was thrown. This is likely due to invalid data in the request body, such as null values.");
                _logger.LogError(ex.Message);

                return new BadRequestObjectResult("JSON Error!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new BadRequestObjectResult("Error!");
            }
        }
    }
}
