
using System.IO.Abstractions;
using DW.Website.Models;
using Kekiri.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using LibGit2Sharp;
using DW.Website.Factories;

namespace DW.Website.Services.Tests;

public class CrosspostServiceScenarios : Scenarios, IDisposable
{
    BlogPost _blogPost;
    CrosspostService _service;
    string repositoryDirectory = Path.Join(Path.GetTempPath(), "testrepo");

    Mock<ILogger> _mockLogger;
    Mock<IFileSystem> _mockFileSystem;
    Mock<IFile> _mockFile;
    Mock<IGitRepositoryFactory> _mockRepositoryFactory;
    Mock<IRepository> _mockRepository;
    Mock<LibGit2Sharp.Index> _mockIndex;
    Mock<Network> _mockNetwork;
    Mock<Branch> _mockBranch;


    public CrosspostServiceScenarios()
    {
        // _service = new CrosspostService();
        _blogPost = new BlogPost();
        
        _mockLogger = new Mock<ILogger>();

        _mockFileSystem = new Mock<IFileSystem>();
        _mockFile = new Mock<IFile>();
        _mockFileSystem.Setup(m => m.File).Returns(_mockFile.Object);

        _mockRepositoryFactory = new Mock<IGitRepositoryFactory>();
        _mockRepository = new Mock<IRepository>();
        _mockIndex = new Mock<LibGit2Sharp.Index>();
        _mockNetwork = new Mock<Network>();
        _mockBranch = new Mock<Branch>();

        _mockNetwork.Setup(m => m.Push(It.IsAny<Branch>()));
        _mockRepository.Setup(m => m.Branches[It.IsAny<string>()]).Returns(_mockBranch.Object);
        _mockRepository.Setup(m => m.Index).Returns(_mockIndex.Object);
        _mockRepository.Setup(m => m.Network).Returns(_mockNetwork.Object);
        _mockRepositoryFactory.Setup(m => m.Create(It.IsAny<string>())).Returns(_mockRepository.Object);
        
        this.CleanUp();
    }

    public void Dispose()
    {
        this.CleanUp();
    }
    void CleanUp()
    {
        if(Directory.Exists(repositoryDirectory))
        {
            Directory.Delete(repositoryDirectory, true);
        }
    }

    [Scenario]
    public void Crosspost_To_WesternDevs()
    {
        Given(a_crosspostservice)
            .And(a_valid_blog_post);
        When(crossposting_to_westerndevs);
        Then(the_repsitory_is_cloned)
            .And(the_post_is_added)
            .And(the_changes_are_committed_to_the_repo)
            .And(the_changes_pushed);
    }

    #region Steps

    void a_crosspostservice()
    {
        // _service = new CrosspostService(_mockFileSystem.Object, _mockLogger.Object, _mockRepositoryFactory.Object);
    }

    void a_valid_blog_post()
    {
        _blogPost = new BlogPost()
        {
            Title = "My test blog post",
            Description = "My test blog post description",
            OriginalURL = "https://www.davidwesst.com/blog/testpost",
            Slug = "testpostslug",
            PublishDate = DateTime.Now,
            HTMLContent = "<h1>My test blog post</h1><p>Content goes here</p>"
        };
    }

    void crossposting_to_westerndevs()
    {
        _service.CrosspostToWesternDevs(_blogPost);
    }

    void the_repsitory_is_cloned()
    {
        _mockRepositoryFactory.Verify(service => service.Clone(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        //Assert.True(Directory.Exists(repositoryDirectory));
        //Assert.True(Directory.GetFiles(repositoryDirectory).Length != 0);
    }

    void the_post_is_added()
    {
        var expectedPostLocation = Path.Join(repositoryDirectory, CrosspostService.WD_POST_DIRECTORY, "test-post.md");
        _mockFileSystem.Verify(service => service.File.WriteAllText(expectedPostLocation, It.IsAny<string>()), Times.Once());
        
        //Assert.True(File.Exists(expectedPostLocation));
    }

    void the_changes_are_committed_to_the_repo()
    {
        _mockRepository.Verify(service => service.Commit(It.IsAny<string>(), It.IsAny<Signature>(), It.IsAny<Signature>(), It.IsAny<CommitOptions>()), Times.Once());
    }

    void the_changes_pushed()
    {
        _mockRepository.Verify(service => service.Network.Push(It.IsAny<Branch>()));
    }

    #endregion

}