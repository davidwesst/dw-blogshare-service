using System.Text.Json;
using System.Text.Json.Serialization;
using DW.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;
using HtmlAgilityPack;

namespace DW.Website;

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
            BlogPost? blogPost = JsonSerializer.Deserialize<BlogPost>(requestBody);

            if(blogPost != null && string.IsNullOrEmpty(blogPost.MDContent))
            {
                _logger.LogInformation("Converting HTML to fill in MDContent property");
                blogPost.MDContent = ConvertHTMLtoMD(blogPost.HTMLContent);
            }

            if(blogPost != null && blogPost.MediaURLs.Length == 0)
            {
                _logger.LogInformation("Extracting media URLs from HTML");
                blogPost.MediaURLs = ExtractMediaUrls(blogPost.HTMLContent);
            }

            if(blogPost != null && blogPost.LastUpdatedDate == DateTime.MinValue)
            {
                blogPost.LastUpdatedDate = blogPost.PublishDate;
            }

            _logger.LogInformation("Processed Normalize Request.");

            return new OkObjectResult(blogPost);
        }
        catch (JsonException ex)
        {
            var errorMsg = "A JsonException was thrown. This is likely due to invalid data in the request body, such as null values.";

            _logger.LogError(errorMsg);
            _logger.LogError(ex.Message);

            return new BadRequestObjectResult(errorMsg);
        }
        catch (Exception ex)
        {
            var errorMsg = "An unknown error was thrown when processing. Check logs for details.";

            _logger.LogError(ex.ToString());
            return new BadRequestObjectResult(errorMsg);
        }
    }

    private string ConvertHTMLtoMD(string htmlContent)
    {
        var converter = new ReverseMarkdown.Converter();
        return converter.Convert(htmlContent);
    }

    public static string[] ExtractMediaUrls(string htmlContent)
    {
        var mediaUrls = new List<string>();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // Extract images
        var imageNodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");
        if (imageNodes != null)
        {
            mediaUrls.AddRange(imageNodes.Select(node => node.GetAttributeValue("src", string.Empty)));
        }

        // Extract videos
        var videoNodes = htmlDoc.DocumentNode.SelectNodes("//video/source[@src]");
        if (videoNodes != null)
        {
            mediaUrls.AddRange(videoNodes.Select(node => node.GetAttributeValue("src", string.Empty)));
        }

        // Extract audio
        var audioNodes = htmlDoc.DocumentNode.SelectNodes("//audio/source[@src]");
        if (audioNodes != null)
        {
            mediaUrls.AddRange(audioNodes.Select(node => node.GetAttributeValue("src", string.Empty)));
        }

        return mediaUrls.ToArray();
    }
}
