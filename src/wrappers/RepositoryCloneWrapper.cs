namespace DW.Website.Wrappers;

using LibGit2Sharp;

public interface IRepositoryCloneWrapper
{
    string Clone(string sourceUrl, string workdirPath);
}

public class RepositoryCloneWrapper : IRepositoryCloneWrapper
{
    public string Clone(string sourceUrl, string workdirPath)
    {
        return Repository.Clone(sourceUrl, workdirPath);
    }
}