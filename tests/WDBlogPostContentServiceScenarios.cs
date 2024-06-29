namespace DW.Website.Services.Tests;

using DW.Website.Models;
using DW.Website.Services;
using Kekiri.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class WDBlogPostContentServiceScenarios : Scenarios, IDisposable
{
    BlogPost _blogPost;
    WDBlogPostContentService _service;
    string _resultTitle;

    public WDBlogPostContentServiceScenarios()
    {
        _blogPost = new BlogPost();
        _resultTitle = String.Empty;
        _service = new WDBlogPostContentService();

        CleanUp();
    }

    public void Dispose()
    {
        CleanUp();
    }

    [Scenario]
    public void GeneratesWDFormattedTitle()
    {
        Given(a_service)
            .And(a_blog_post);
        When(generating_a_blog_post_title);
        Then(it_starts_with_the_date_and_ends_with_the_post_slug);
    }

    #region Setup & Teardown Methods

    void CleanUp()
    {

    }

    #endregion

    #region Given Steps

    void a_service()
    {
        _service = new WDBlogPostContentService();
    }

    void a_blog_post()
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

    #endregion

    #region When Steps

    void generating_a_blog_post_title()
    {
        _resultTitle = _service.GenerateBlogPostFileName(_blogPost);
    }

    #endregion

    #region Then Steps

    void it_starts_with_the_date_and_ends_with_the_post_slug()
    {
        var expectedYear = _blogPost.PublishDate.Year;
        var expectedMonth = _blogPost.PublishDate.Month;
        var expectedDay = _blogPost.PublishDate.Day;
        
        var expectedFileName = $"{expectedYear}-{expectedMonth}-{expectedDay}-{_blogPost.Slug}";

        Assert.Equal(expectedFileName, _resultTitle);
    }

    #endregion
}