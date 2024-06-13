using DW.Website.Models;
using LibGit2Sharp;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;
using System.IO;

namespace DW.Website.Services;

public class CrosspostService
{
    private readonly ILogger _logger;

    public CrosspostService(ILogger logger)
    {
        _logger = logger;
    }

    public CrosspostService()
    {
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger(typeof(CrosspostService));
    }
    
    public void CrosspostToWesternDevs(BlogPost post)
    {
        // prep temp directory
        var repoDirectory = Path.Join(Path.GetTempPath(), "testrepo");
        _logger.LogInformation(repoDirectory);

        // clone repository
        var wdRepository = Repository.Clone("https://github.com/westerndevs/western-devs-website", repoDirectory);

        // 
    }
}