using DW.Website.Models;
using DW.Website.Services;
using Microsoft.Extensions.Logging;

namespace DW.Website.Services;

public class CrosspostService(IBlogPostContentService contentService, GitHelperService gitHelperService)
{
    public static readonly string WD_POST_DIRECTORY = @"./source/_posts/";
    public static readonly string WD_REPO_URL = @"https://github.com/westerndevs/western-devs-website";
    private readonly string WD_AUTHOR = "david_wesst";

    public CrosspostService() : this(new WDBlogPostContentService(), new GitHelperService()) { }

    public void CrosspostToWesternDevs(BlogPost post)
    {
        var postFileName = contentService.GenerateBlogPostFileName(post);
        var postContent = contentService.GenerateBlogPostContent(post, WD_AUTHOR);
        var postRelativePath = Path.Join(WD_POST_DIRECTORY, postFileName);
        gitHelperService.AddFileToRepo(WD_REPO_URL, postRelativePath, postContent, "test message", "main");
    }
}