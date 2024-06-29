namespace DW.Website.Services;

using DW.Website.Models;

public interface IBlogPostContentService
{
    public string GenerateBlogPostContent(BlogPost post, string author);
    public string GenerateBlogPostFileName(BlogPost post);
}