using DW.Website.Models;
using LibGit2Sharp;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;
using System.IO;
using System.IO.Abstractions;
using DW.Website.Factories;

namespace DW.Website.Services;

public class CrosspostService
{
    private readonly ILogger _logger;
    private readonly IFileSystem _fs;
    private readonly IGitRepositoryFactory _repoFactory;

    public static readonly string WD_POST_DIRECTORY = @"./source/_posts/";
    public static readonly string WD_REPO_URL = @"https://github.com/westerndevs/western-devs-website";
    private readonly string WD_AUTHOR = "david_wesst";

    public CrosspostService(IFileSystem fs, ILogger logger, IGitRepositoryFactory repoFactory)
    {
        _fs = fs;
        _logger = logger;
        _repoFactory = repoFactory;
    }

    public CrosspostService()
    {
        _fs = new FileSystem(); 
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger(typeof(CrosspostService));
        _repoFactory = new GitRepositoryFactory();
    }
    
    public void CrosspostToWesternDevs(BlogPost post)
    {
        // prep temp directory
        var repoDirectory = Path.Join(Path.GetTempPath(), "testrepo");
        _logger.LogInformation(repoDirectory);

        // clone repository
        //var wdRepository = Repository.Clone("https://github.com/westerndevs/western-devs-website", repoDirectory);
        var wdRepositoryPath = _repoFactory.Clone(WD_REPO_URL, repoDirectory);

        // add markdown post to directory
        var postFileName = "test-post.md";
        var frontMatter = $@"---
        title: {post.Title}
        date: {post.PublishDate.ToString()}
        originalurl: {post.OriginalURL}
        authorId: {WD_AUTHOR}
        ---";
        var markdownContent = post.MDContent;
        var postContent = frontMatter + "\n\n" + markdownContent;
        var postFilePath = Path.Join(repoDirectory, WD_POST_DIRECTORY, postFileName);

        _fs.File.WriteAllText(postFilePath, postContent);

        // stage the post file
        var repo = _repoFactory.Create(wdRepositoryPath);
        repo.Index.Add(postFilePath);

        // create file scaffold for images
        if(post.MediaURLs.Length > 0)
        {
            // add media directory

            // add media

            // store URLs of new images (key (old image URL), value (new image URL))

            // update URLs in post
        }

        // commit file to repo
        repo.Commit("test message", null, null);

        // push repo to origin
        //var remote = repo.Network.Remotes["origin"];
        var branch = repo.Branches["main"];
        repo.Network.Push(branch);
    }
}