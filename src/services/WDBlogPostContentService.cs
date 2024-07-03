namespace DW.Website.Services;

using DW.Website.Models;
using ReverseMarkdown;

public class WDBlogPostContentService: IBlogPostContentService
{
    public static readonly string WD_MEDIA_DIRECTORY = "/images/";

    public string GenerateBlogPostFileName(BlogPost post) 
    {
        return $"{post.PublishDate.Year}-{post.PublishDate.Month}-{post.PublishDate.Day}-{post.Slug}";
    }

    public Dictionary<string, string> ConvertMediaUrls(BlogPost post)
    {
        var updatedUrls = new Dictionary<string, string>();

        foreach (var url in post.MediaURLs)
        {
            var wdUrl = this.ConvertMediaUrl(GenerateBlogPostFileName(post), Path.GetFileName(url));
            updatedUrls.Add(url, wdUrl);
        }

        return updatedUrls;
    }

    public string GenerateBlogPostContent(BlogPost post, string author)
    {
        var frontMatter = $"---\n" +
        $"title: \"{post.Title}\"\n" +
        $"date: \"{post.PublishDate.ToString("u")}\"\n" +
        $"description: \"{post.Description}\n" +
        $"excerpt: \"{post.Excerpt}\"\n" +
        $"categories: \n  - {string.Join("\n  - ", post.Categories)}\n" +
        $"tags: \n  - {string.Join("\n  - ", post.Tags)}\"\n" +
        $"originalurl: \"{post.OriginalURL}\"\n" +
        $"authorId: \"{author}\"\n" +
        "---";
        
        var markdownContent = this.GenerateMDfromHTML(post.HTMLContent);
        foreach (var originalUrl in post.MediaURLs)
        {
            markdownContent = this.UpdateMediaUrl(markdownContent, this.GenerateBlogPostFileName(post), originalUrl);
        }
        
        var postContent = frontMatter + "\n\n" + markdownContent;

        return postContent;
    }

    private string GenerateMDfromHTML(string html)
    {
        var htmlConverter = new ReverseMarkdown.Converter();

        return htmlConverter.Convert(html);
    }

    public string ConvertMediaUrl(string postFileName, string originalMediaUrl)
    {
        var wdUrl = Path.Join(WD_MEDIA_DIRECTORY, postFileName, Path.GetFileName(originalMediaUrl));

        return wdUrl;
    }

    public string UpdateMediaUrl(string contentString, string postFileName, string originalMediaUrl)
    {
        var wdUrl = this.ConvertMediaUrl(postFileName, originalMediaUrl);
        return contentString.Replace(originalMediaUrl, wdUrl);
    }
}