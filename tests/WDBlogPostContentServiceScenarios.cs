namespace DW.Website.Services.Tests;

using System.ComponentModel;
using System.Text.RegularExpressions;
using DW.Website.Models;
using DW.Website.Services;
using Kekiri.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using ReverseMarkdown.Converters;
using Xunit;

public class WDBlogPostContentServiceScenarios : Scenarios, IDisposable
{
    BlogPost _blogPost;
    WDBlogPostContentService _service;
    string _result = String.Empty;
    string _testAuthorId = String.Empty;

    public WDBlogPostContentServiceScenarios()
    {
        _blogPost = new BlogPost();
        _service = new WDBlogPostContentService();

        CleanUp();
    }

    public void Dispose()
    {
        CleanUp();
    }

    [Scenario]
    public void Generates_a_WD_Formatted_File_Name()
    {
        Given(a_service)
            .And(a_blog_post);
        When(generating_a_blog_post_title);
        Then(it_starts_with_the_date_and_ends_with_the_post_slug);
    }

    [Scenario]
    public void Generates_WD_Formatted_Content()
    {
        Given(a_service)
            .And(a_blog_post)
            .And(an_author_id);
        When(generating_the_blog_post_contents);
        Then(it_is_valid_markdown)
            .And(always_has_frontmatter);
    }

    [Scenario]
    public void Includes_WD_FrontMatter_Values()
    {
        Given(a_service)
            .And(a_blog_post_with_all_properties)
            .And(an_author_id);
        When(generating_the_blog_post_contents);
        Then(it_has_frontmatter_with_a_title)
            .And(it_has_frontmatter_with_a_description)
            .And(it_has_frontmatter_with_an_excerpt)
            .And(it_has_frontmatter_with_the_author_id)
            .And(it_has_frontmatter_with_the_publish_date_in_iso8601_format)
            .And(it_has_frontmatter_with_the_original_url)
            .And(it_has_frontmatter_with_categories)
            .And(it_has_frontmatter_with_tags);
    }
    /**
    [Scenario]
    public void Updates_Media_Urls_to_WD_Relative_Locations()
    {
        Given(a_service)
            .And(a_blog_post_with_media)
            .And(an_author_id);
        When(generating_the_blog_post_contents);
        Then(all_media_urls_are_updated);
    }
    **/

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

    void a_blog_post_with_all_properties()
    {
        _blogPost = new BlogPost()
        {
            Title = "My test blog post",
            Description = "My test blog post description",
            OriginalURL = "https://www.davidwesst.com/blog/testpost",
            Slug = "testpostslug",
            PublishDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now,
            HTMLContent = "<h1>This is a test post</h1><p>  This is the content with an image. <img src=\"images/test_image.png\" alt=\"alt text here\" title=\"Title\" /> More text.</p>",
            MDContent = "# This is a test post\nThis is the content with an image.![alt text here](images/test_image.png \"Title\")More text.",
            Categories = [ "category1", "category2" ],
            Tags = [ "tag1", "tag2" ],
            MediaURLs = [ "images/test_images.png" ]
        };
   }

   void an_author_id()
   {
        _testAuthorId = "test_author_id";
   }

    #endregion

    #region When Steps

    void generating_a_blog_post_title()
    {
        _result = _service.GenerateBlogPostFileName(_blogPost);
    }

    void generating_the_blog_post_contents()
    {
        _result = _service.GenerateBlogPostContent(_blogPost, _testAuthorId);
    }

    #endregion

    #region Then Steps

    void it_starts_with_the_date_and_ends_with_the_post_slug()
    {
        var expectedYear = _blogPost.PublishDate.Year;
        var expectedMonth = _blogPost.PublishDate.Month;
        var expectedDay = _blogPost.PublishDate.Day;
        
        var expectedFileName = $"{expectedYear}-{expectedMonth}-{expectedDay}-{_blogPost.Slug}";

        Assert.Equal(expectedFileName, _result);
    }

    void it_is_valid_markdown()
    {
        Assert.NotNull(_blogPost.MDContent);
        Assert.Contains("# My test blog post\n\nContent goes here", _result);
    }

    void always_has_frontmatter()
    {
        var yamlPattern = @"^---\s*\n.*?\n---\s*\n";
        var yamlRegex = new Regex(yamlPattern, RegexOptions.Singleline);

        Assert.Matches(yamlRegex, _result);
    }
    
    void it_has_frontmatter_with_a_title()
    {
        var pattern = @"^---.*?title: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }

    void it_has_frontmatter_with_a_description()
    {
        var pattern = @"^---.*?description: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }
    
    void it_has_frontmatter_with_an_excerpt()
    {
        var pattern = @"^---.*?excerpt: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }
    
    void it_has_frontmatter_with_the_author_id()
    {
        var pattern = @"^---.*?authorId: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }

    void it_has_frontmatter_with_the_publish_date_in_iso8601_format()
    {
        var pattern = @"^---.*?date: ""(\d{4})-(\d{2})-(\d{2})[T\s](\d{2}):(\d{2}):(\d{2})(\.\d+)?(Z|([+-](\d{2}):?(\d{2})))?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }

    void it_has_frontmatter_with_the_original_url()
    {
        var pattern = @"^---.*?originalurl: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }

    void it_has_frontmatter_with_categories()
    {
        var pattern = @"^---\n(?:.*\n)*?categories:\s*\n((?:\s+-\s+.+\n)+)(?:.*\n)*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }
    
    void it_has_frontmatter_with_tags()
    {
        var pattern = @"^---\n(?:.*\n)*?tags:\s*\n((?:\s+-\s+.+\n)+)(?:.*\n)*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _result);
    }
    
    #endregion
}