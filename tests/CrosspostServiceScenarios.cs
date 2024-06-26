
using System.IO.Abstractions;
using DW.Website.Models;
using Kekiri.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DW.Website.Services.Tests;

public class CrosspostServiceScenarios : Scenarios, IDisposable
{
    BlogPost _blogPost;
    CrosspostService _service;
    string repositoryDirectory = Path.Join(Path.GetTempPath(), "testrepo");

    public CrosspostServiceScenarios()
    {
        _service = new CrosspostService();
        _blogPost = new BlogPost();

        this.CleanUp();
    }

    public void Dispose()
    {
        this.CleanUp();
    }

    [Scenario]
    public void This_should_pass()
    {
        Given(something);
        When(it_happens);
        Then(you_will_know);
    }

    void something()
    {
        var something = true;
    }

    void it_happens()
    {
        var another_something = true;
    }

    void you_will_know()
    {
        Assert.True(true);
    }

    [Scenario(Skip = "Not a unit test")]
    public void Cloning_a_repository()
    {
        Given(a_crosspostservice)
            .And(a_valid_blog_post);
        When(crossposting);
        Then(the_repsitory_is_cloned);
    }

    void a_crosspostservice()
    {
        var mockLogger = new Mock<ILogger>();
        var mockFileSystem = new Mock<IFileSystem>();
        
        _service = new CrosspostService(mockFileSystem.Object, mockLogger.Object);
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

    void crossposting()
    {
        _service.CrosspostToWesternDevs(_blogPost);
    }

    void the_repsitory_is_cloned()
    {
        var repoDir = Path.Join(Path.GetTempPath(), "testrepo");
        Assert.True(Directory.Exists(repoDir));
        Assert.True(Directory.GetFiles(repoDir).Length != 0);
    }

    void CleanUp()
    {
        if(Directory.Exists(repositoryDirectory))
        {
            Directory.Delete(repositoryDirectory, true);
        }
    }
}