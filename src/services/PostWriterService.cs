using DW.Website.Models;

namespace DW.Website.Services;

public class PostWriterService
{
    public string WritePostContents(BlogPost post, string author)
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