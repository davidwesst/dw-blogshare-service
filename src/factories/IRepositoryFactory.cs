namespace DW.Website.Factories;

using LibGit2Sharp;

public interface IRepositoryFactory
{
    IRepository Create(string path);
}