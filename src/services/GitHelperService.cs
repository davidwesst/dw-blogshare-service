using System.IO.Abstractions;
using DW.Website.Factories;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace DW.Website.Services;

public class GitHelperService(IFileSystem fs, ILogger logger, IGitRepositoryFactory repoFactory)
{
    private readonly ILogger _logger = logger;
    private readonly IFileSystem _fs = fs;
    private readonly IGitRepositoryFactory _repoFactory = repoFactory;

    public void AddFileToRepo(string repoUrl, string relativeFilePath, string fileContents, string commitMessage, string branchName)
    {
        // prep temp directory
        var repoDirectory = Path.Join(Path.GetTempPath(), "testrepo");
        _logger.LogInformation(repoDirectory);

        // clone repository
        var clonePath = _repoFactory.Clone(repoUrl, repoDirectory);
        var newFilePath = Path.Join(clonePath, relativeFilePath);

        _fs.File.WriteAllText(newFilePath, fileContents);

        // stage the post file
        var repo = _repoFactory.Create(clonePath);
        repo.Index.Add(newFilePath);

        // commit file to repo
        repo.Commit(commitMessage, null, null);

        // push repo to origin
        var branch = repo.Branches[branchName];
        repo.Network.Push(branch);
    }
}