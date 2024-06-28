namespace DW.Website.Factories;

using LibGit2Sharp;

public interface IGitRepositoryFactory
{
    IRepository Create(string path);
    string Clone(string sourceUrl, string workdirPath);
}