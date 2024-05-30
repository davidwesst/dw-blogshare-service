using System.Text.Json.Serialization;

namespace DW.Website.Models;

public class BlogPost
{
    [JsonPropertyName("title")]
    [JsonRequired]
    public string Title { get; set;} = "";
    [JsonPropertyName("description")]
    [JsonRequired]
    public string Description { get; set; } = "";
    [JsonPropertyName("excerpt")]
    public string Excerpt { get; set; } = "";
    [JsonPropertyName("originalURL")]
    [JsonRequired]
    public string OriginalURL { get; set; } = "";
    [JsonPropertyName("slug")]
    [JsonRequired]
    public string Slug { get; set; } = "";
    [JsonPropertyName("tags")]
    public string[] Tags { get; set; } = Array.Empty<string>();
    [JsonPropertyName("categories")]
    public string[] Categories { get; set; } = Array.Empty<string>();
    [JsonPropertyName("publishDate")]
    [JsonRequired]
    public DateTime PublishDate { get; set; } = DateTime.MinValue;
    [JsonPropertyName("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; } = DateTime.MinValue;
    [JsonPropertyName("htmlContent")]
    [JsonRequired]
    public string HTMLContent { get; set; } = "";
    [JsonPropertyName("mdContent")]
    public string MDContent { get; set; } = "";
    [JsonPropertyName("mediaURLs")]
    public string[] MediaURLs { get; set; } = Array.Empty<string>();
}