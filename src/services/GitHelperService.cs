using System.IO.Abstractions;
using DW.Website.Factories;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace DW.Website.Services;

public class GitHelperService(IFileSystem fs, ILogger logger, IGitRepositoryFactory repoFactory)
{
    public void AddFileToRepo(string repoUrl, string relativeFilePath, string fileContents, string commitMessage, string branchName)
    {
        // prep temp directory
        var repoDirectory = Path.Join(Path.GetTempPath(), "testrepo");
        logger.LogInformation(repoDirectory);

        // clone repository
        var clonePath = repoFactory.Clone(repoUrl, repoDirectory);
        var newFilePath = Path.Join(clonePath, relativeFilePath);

        fs.File.WriteAllText(newFilePath, fileContents);

        // stage the post file
        var repo = repoFactory.Create(clonePath);
        repo.Index.Add(newFilePath);

        // commit file to repo
        repo.Commit(commitMessage, null, null);

        // push repo to origin
        var branch = repo.Branches[branchName];
        repo.Network.Push(branch);
    }
}