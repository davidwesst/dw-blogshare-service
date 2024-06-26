using DW.Website.Models;
using LibGit2Sharp;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;
using System.IO;
using System.IO.Abstractions;

namespace DW.Website.Services;

public class CrosspostService
{
    private readonly ILogger _logger;
    private readonly IFileSystem _fs;

    public CrosspostService(IFileSystem fs, ILogger logger)
    {
        _fs = fs;
        _logger = logger;
    }

    public CrosspostService()
    {
        _fs = new FileSystem(); 
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger(typeof(CrosspostService));
    }
    
    public void CrosspostToWesternDevs(BlogPost post)
    {
        // prep temp directory
        var repoDirectory = Path.Join(Path.GetTempPath(), "testrepo");
        _logger.LogInformation(repoDirectory);

        // clone repository
        var wdRepository = Repository.Clone("https://github.com/westerndevs/western-devs-website", repoDirectory);
    }
}