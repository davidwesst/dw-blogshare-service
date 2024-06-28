namespace DW.Website.Factories;

using LibGit2Sharp;

public class GitRepositoryFactory : IGitRepositoryFactory
{
    public IRepository Create(string path)
    {
        return new Repository(path);
    }

    public string Clone(string sourceUrl, string workdirPath)
    {
        return Repository.Clone(sourceUrl, workdirPath);
    }
}