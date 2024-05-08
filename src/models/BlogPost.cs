using Newtonsoft.Json;
using System;

namespace DW.Website.BlogShare.Models;

public class BlogPost
{
    public string Title { get; set;} = "";
    public string Description { get; set; } = "";
    public string Excerpt { get; set; } = "";
    public string OriginalURL { get; set; } = "";
    public string Slug { get; set; } = "";
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string[] Categories { get; set; } = Array.Empty<string>();
    public DateTime PublishDate { get; set; } = DateTime.MinValue;
    public DateTime LastUpdatedDate { get; set; } = DateTime.MinValue;
    public string HTMLContent { get; set; } = "";
    public string MDContent { get; set; } = "";
    public string[] MediaURLs { get; set; } = Array.Empty<string>();
}