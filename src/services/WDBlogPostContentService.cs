namespace DW.Website.Services;

using DW.Website.Models;

public class WDBlogPostContentService: IBlogPostContentService
{
    public string GenerateBlogPostFileName(BlogPost post)
    {
        return "test-post";
    }

    public string GenerateBlogPostContent(BlogPost post, string author)
    {
        var frontMatter = $@"---
        title: {post.Title}
        date: {post.PublishDate.ToString()}
        originalurl: {post.OriginalURL}
        authorId: {author}
        ---";
        var markdownContent = post.MDContent;
        var postContent = frontMatter + "\n\n" + markdownContent;

        return postContent;
    }
}