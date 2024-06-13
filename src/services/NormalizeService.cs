using DW.Website.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;

namespace DW.Website.Services;

public class NormalizeService
{
    private readonly ILogger _logger;

    public NormalizeService(ILogger logger) 
    {
        _logger = logger;
    }

    public BlogPost Normalize(BlogPost post)
    {
        BlogPost normalizedPost = post;
        
        if(string.IsNullOrEmpty(post.MDContent))
        {
            _logger.LogInformation("Converting HTML to fill in MDContent property");
            normalizedPost.MDContent = ConvertHTMLtoMD(post.HTMLContent);
        }

        if(post.MediaURLs.Length == 0)
        {
            _logger.LogInformation("Extracting media URLs from HTML");
            normalizedPost.MediaURLs = ExtractMediaUrls(post.HTMLContent);
        }

        if(post != null && post.LastUpdatedDate == DateTime.MinValue)
        {
            normalizedPost.LastUpdatedDate = post.PublishDate;
        }

        return normalizedPost;
    }
    
    internal string ConvertHTMLtoMD(string htmlContent)
    {
        var converter = new ReverseMarkdown.Converter();
        return converter.Convert(htmlContent);
    }

    internal static string[] ExtractMediaUrls(string htmlContent)
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