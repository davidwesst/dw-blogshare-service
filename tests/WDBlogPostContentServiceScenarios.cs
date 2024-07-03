namespace DW.Website.Services.Tests;

using System.ComponentModel;
using System.Text.RegularExpressions;
using DW.Website.Models;
using DW.Website.Services;
using Kekiri.Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.Frameworks;
using ReverseMarkdown.Converters;
using Xunit;

public class WDBlogPostContentServiceScenarios : Scenarios, IDisposable
{
    BlogPost _testBlogPost;
    WDBlogPostContentService _service;
    string _resultString = String.Empty;
    string _resultUrl = String.Empty;
    string[] _resultUrls = [];
    string _testAuthorId = String.Empty;

    public WDBlogPostContentServiceScenarios()
    {
        _testBlogPost = new BlogPost();
        _service = new WDBlogPostContentService();

        CleanUp();
    }

    #region Setup & Teardown Methods

    void CleanUp()
    {
        // clear results
        _resultString = String.Empty;
        _resultUrl = String.Empty;
    }

    public void Dispose()
    {
        CleanUp();
    }
    
    #endregion


    #region Scenarios

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

    [Scenario]
    public void Converts_Media_Url_to_WD_Url()
    {
        Given(a_service)
            .And(a_blog_post_with_all_properties);
        When(coverting_a_media_url);
        Then(the_new_url_has_the_same_file_name)
            .And(the_new_url_is_a_post_specific_folder_in_the_WD_media_directory);
    }

    [Scenario]
    public void Coverts_Media_Url_Array_to_WD_Urls()
    {
        Given(a_service)
            .And(a_blog_post_with_all_properties);
        When(converting_an_array_of_media_urls);
        Then(returns_an_array_of_updated_urls_that_share_indexes);
    }

    [Scenario]
    public void Updates_Media_Urls_to_WD_Relative_Locations()
    {
        Given(a_service)
            .And(a_blog_post_with_all_properties)
            .And(an_author_id);
        When(generating_the_blog_post_contents);
        Then(all_media_urls_are_updated);
    }

    #endregion

    #region Given Steps

    void a_service()
    {
        _service = new WDBlogPostContentService();
    }

    void a_blog_post()
    {
        _testBlogPost = new BlogPost()
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
        _testBlogPost = new BlogPost()
        {
            Title = "My test blog post",
            Description = "My test blog post description",
            OriginalURL = "https://www.davidwesst.com/blog/testpost",
            Slug = "testpostslug",
            PublishDate = DateTime.Now,
            LastUpdatedDate = DateTime.Now,
            HTMLContent = "<h1>This is a test post</h1><p>  This is the content with an image. <img src=\"test_location/test_image.png\" alt=\"alt text here\" title=\"Title\" /> More text.</p>",
            MDContent = "# This is a test post\nThis is the content with an image.![alt text here](test_location/test_image.png \"Title\")More text.",
            Categories = [ "category1", "category2" ],
            Tags = [ "tag1", "tag2" ],
            MediaURLs = [ "test_location/test_image.png" ]
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
        _resultString = _service.GenerateBlogPostFileName(_testBlogPost);
    }

    void generating_the_blog_post_contents()
    {
        _resultString = _service.GenerateBlogPostContent(_testBlogPost, _testAuthorId);
    }

    void coverting_a_media_url()
    {
        var blogPostFileName = _service.GenerateBlogPostFileName(_testBlogPost);
        _resultUrl = _service.ConvertMediaUrl(blogPostFileName, _testBlogPost.MediaURLs[0]);
    }

    void converting_an_array_of_media_urls()
    {
        var blogPostFileName = _service.GenerateBlogPostFileName(_testBlogPost);
        _resultUrls = _service.ConvertMediaUrls(blogPostFileName, _testBlogPost.MediaURLs);
    }

    #endregion

    #region Then Steps

    void it_starts_with_the_date_and_ends_with_the_post_slug()
    {
        var expectedYear = _testBlogPost.PublishDate.Year;
        var expectedMonth = _testBlogPost.PublishDate.Month;
        var expectedDay = _testBlogPost.PublishDate.Day;
        
        var expectedFileName = $"{expectedYear}-{expectedMonth}-{expectedDay}-{_testBlogPost.Slug}";

        Assert.Equal(expectedFileName, _resultString);
    }

    void it_is_valid_markdown()
    {
        Assert.NotNull(_testBlogPost.MDContent);
        Assert.Contains("# My test blog post\n\nContent goes here", _resultString);
    }

    void always_has_frontmatter()
    {
        var yamlPattern = @"^---\s*\n.*?\n---\s*\n";
        var yamlRegex = new Regex(yamlPattern, RegexOptions.Singleline);

        Assert.Matches(yamlRegex, _resultString);
    }
    
    void it_has_frontmatter_with_a_title()
    {
        var pattern = @"^---.*?title: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }

    void it_has_frontmatter_with_a_description()
    {
        var pattern = @"^---.*?description: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }
    
    void it_has_frontmatter_with_an_excerpt()
    {
        var pattern = @"^---.*?excerpt: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }
    
    void it_has_frontmatter_with_the_author_id()
    {
        var pattern = @"^---.*?authorId: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }

    void it_has_frontmatter_with_the_publish_date_in_iso8601_format()
    {
        var pattern = @"^---.*?date: ""(\d{4})-(\d{2})-(\d{2})[T\s](\d{2}):(\d{2}):(\d{2})(\.\d+)?(Z|([+-](\d{2}):?(\d{2})))?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }

    void it_has_frontmatter_with_the_original_url()
    {
        var pattern = @"^---.*?originalurl: "".*?"".*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }

    void it_has_frontmatter_with_categories()
    {
        var pattern = @"^---\n(?:.*\n)*?categories:\s*\n((?:\s+-\s+.+\n)+)(?:.*\n)*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }
    
    void it_has_frontmatter_with_tags()
    {
        var pattern = @"^---\n(?:.*\n)*?tags:\s*\n((?:\s+-\s+.+\n)+)(?:.*\n)*?---";
        var regex = new Regex(pattern, RegexOptions.Singleline);

        Assert.Matches(regex, _resultString);
    }

    void all_media_urls_are_updated()
    {
        var updatedUrls = _service.ConvertMediaUrls(_service.GenerateBlogPostFileName(_testBlogPost), _testBlogPost.MediaURLs);
        for(var itemIndex = 0; itemIndex < _testBlogPost.MediaURLs.Length; itemIndex++)
        {
            Assert.DoesNotContain(_testBlogPost.MediaURLs[itemIndex], _resultString);
            Assert.Contains(updatedUrls[itemIndex], _resultString);
        }
    }

    void the_new_url_has_the_same_file_name()
    {
        Assert.Equal(Path.GetFileName(_testBlogPost.MediaURLs[0]), Path.GetFileName(_resultUrl));
    }

    void the_new_url_is_a_post_specific_folder_in_the_WD_media_directory()
    {
        var expectedPath = Path.Join($"{WDBlogPostContentService.WD_MEDIA_DIRECTORY}", _service.GenerateBlogPostFileName(_testBlogPost));
        Assert.StartsWith(expectedPath, _resultUrl);
    }


    void returns_an_array_of_updated_urls_that_share_indexes()
    {
        // check that the same number were created
        Assert.Equal(_testBlogPost.MediaURLs.Length, _resultUrls.Length);

        // check each item based on index
        for (var itemIndex = 0; itemIndex < _resultUrls.Length; itemIndex++)
        {
            var originalItem = _testBlogPost.MediaURLs[itemIndex];
            var updatedItem = _resultUrls[itemIndex];

            // check if updated
            var expectedNewUrl = Path.Join(WDBlogPostContentService.WD_MEDIA_DIRECTORY, _service.GenerateBlogPostFileName(_testBlogPost), Path.GetFileName(updatedItem));
            Assert.Equal(expectedNewUrl, updatedItem);

            // check if file names match, based on index
            Assert.Equal(Path.GetFileName(originalItem), Path.GetFileName(updatedItem));
        }
    }

    #endregion
}