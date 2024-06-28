using DW.Website.Models;

namespace DW.Website.Services;

public class CrosspostService(PostWriterService postWriterService, GitHelperService gitHelperService)
{
    private readonly PostWriterService _postWriterService = postWriterService;
    private readonly GitHelperService _gitHelperService = gitHelperService;

    public static readonly string WD_POST_DIRECTORY = @"./source/_posts/";
    public static readonly string WD_REPO_URL = @"https://github.com/westerndevs/western-devs-website";
    private readonly string WD_AUTHOR = "david_wesst";

    public void CrosspostToWesternDevs(BlogPost post)
    {
        var postFileName = "test-post.md";
        var postContent = _postWriterService.WritePostContents(post, WD_AUTHOR);
        var postRelativePath = Path.Join(WD_POST_DIRECTORY, postFileName);
        _gitHelperService.AddFileToRepo(WD_REPO_URL, postRelativePath, postContent, "test message", "main");
    }
}