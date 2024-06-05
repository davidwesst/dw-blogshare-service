using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace DW.Website.Models;

/// <summary>
/// A Blog Post
/// </summary>
public class BlogPost
{
    [OpenApiProperty(Nullable = false, Default = "", Description = "The title of the blog post.")]
    [JsonPropertyName("title")]
    [JsonRequired]
    public string Title { get; set;} = "";
    
    [OpenApiProperty(Nullable = false, Default = "", Description = "Short description of the blog post.")]
    [JsonPropertyName("description")]
    [JsonRequired]
    public string Description { get; set; } = "";
    
    [OpenApiProperty(Nullable = true, Default = "", Description = "(Optional) Short excerpt from the blog post.")]
    [JsonPropertyName("excerpt")]
    public string Excerpt { get; set; } = "";

    [OpenApiProperty(Nullable = false, Default = "", Description = "The original web URL for the post.")]
    [JsonPropertyName("originalURL")]
    [JsonRequired]
    public string OriginalURL { get; set; } = "";

    [OpenApiProperty(Nullable = false, Default = "", Description = "A user-friendly, URL-safe string that represents the title or a unique identifier of the blog post.")]
    [JsonPropertyName("slug")]
    [JsonRequired]
    public string Slug { get; set; } = "";

    [OpenApiProperty(Nullable = true, Default = "string[]", Description = "(Optional) A set of keywords or phrases that categorize the content and highlight its key topic.")]
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; } = Array.Empty<string>();
    
    [OpenApiProperty(Nullable = true, Default = "string[]", Description = "(Optional) A broader classifications that group related posts together")]
    [JsonPropertyName("categories")]
    public string[] Categories { get; set; } = Array.Empty<string>();
    
    [OpenApiProperty(Nullable = false, Default = "00:00:00.0000000 UTC, January 1, 0001", Description = "The specific date and time when the post was made publicly available on the blog, in ISO 8601 format.")]
    [JsonPropertyName("publishDate")]
    [JsonRequired]
    public DateTime PublishDate { get; set; } = DateTime.MinValue;
    
    [OpenApiProperty(Nullable = true, Default = "00:00:00.0000000 UTC, January 1, 0001", Description = "(Optional) The most recent date and time when the post was edited or updated, in ISO 8601 format.")]
    [JsonPropertyName("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; } = DateTime.MinValue;
    
    [OpenApiProperty(Nullable = false, Default = "", Description = "The full content of the post formatted in HTML.")]
    [JsonPropertyName("htmlContent")]
    [JsonRequired]
    public string HTMLContent { get; set; } = "";
    
    [OpenApiProperty(Nullable = true, Default = "", Description = "T(Optional) he full content of the post written in Markdown (MD) format.")]
    [JsonPropertyName("mdContent")]
    public string MDContent { get; set; } = "";
    
    [OpenApiProperty(Nullable = true, Default = "Array.Empty<string>", Description = "(Optional) A collection of URLs pointing to various media files (audio, video, image) associated with the post.")]
    [JsonPropertyName("mediaURLs")]
    public string[] MediaURLs { get; set; } = Array.Empty<string>();
}