namespace DW.Website.Factories;

using LibGit2Sharp;

public class GitRepositoryFactory : IRepositoryFactory
{
    public IRepository Create(string path)
    {
        return new Repository(path);
    }
}