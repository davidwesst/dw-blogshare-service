namespace DW.Website.Services;

using DW.Website.Models;
using ReverseMarkdown;

public class WDBlogPostContentService: IBlogPostContentService
{
    public string GenerateBlogPostFileName(BlogPost post) 
    {
        return $"{post.PublishDate.Year}-{post.PublishDate.Month}-{post.PublishDate.Day}-{post.Slug}";
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
        var postContent = frontMatter + "\n\n" + markdownContent;

        return postContent;
    }

    private string GenerateMDfromHTML(string html)
    {
        var htmlConverter = new ReverseMarkdown.Converter();

        return htmlConverter.Convert(html);
    }
}