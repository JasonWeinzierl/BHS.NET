using BHS.Infrastructure.Models;
using BHS.Infrastructure.Repositories;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories;

public class PostPreviewRepositoryTests
{
    private readonly PostPreviewRepository _subject;

    private readonly MockExecuter _mockExecuter = new();

    public PostPreviewRepositoryTests()
    {
        _subject = new PostPreviewRepository(_mockExecuter);
    }

    [Fact]
    public async Task Search_Executes()
    {
        _mockExecuter.ManyResults = new PostPreviewCategoryDto[]
        {
            new PostPreviewCategoryDto(
                "b",
                "c",
                "d",

                1,
                string.Empty,
                default,

                "cat1",
                "Category 1",

                new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
        };
        var start = new DateTimeOffset(2020, 12, 14, 20, 43, 0, TimeSpan.FromHours(-6));
        var end = new DateTimeOffset(2020, 12, 14, 20, 44, 0, TimeSpan.FromHours(-6));

        _ = await _subject.Search("a", start, end);

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("blog.PostPreviewCategory_Search", _mockExecuter.CommandText);

        Assert.Equal("a", _mockExecuter.Parameters?.searchText);
        Assert.Equal(start, _mockExecuter.Parameters?.fromDate);
        Assert.Equal(end, _mockExecuter.Parameters?.toDate);
    }

    [Fact]
    public async Task Search_GroupsCategoryRows()
    {
        var row1 = new PostPreviewCategoryDto("x", string.Empty, string.Empty, default, default, default, "cat1", "Thing!", default);
        var row2 = new PostPreviewCategoryDto("x", string.Empty, string.Empty, default, default, default, "cat2", "Thing!", default);
        _mockExecuter.ManyResults = new[] { row1, row2 };

        var results = await _subject.Search("a", default, default);

        Assert.NotNull(results);
        Assert.Equal(1, results.Count);
        Assert.Contains("cat1", results.First().Categories.Select(c => c.Slug));
        Assert.Contains("cat2", results.First().Categories.Select(c => c.Slug));
    }

    [Fact]
    public async Task GetByCategorySlug_Executes()
    {
        _mockExecuter.ManyResults = new PostPreviewCategoryDto[]
        {
            new PostPreviewCategoryDto(
                "b",
                "c",
                "d",

                1,
                string.Empty,
                default,

                "cat1",
                "Category 1",

                default)
        };

        _ = await _subject.GetByCategorySlug("cat");

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("blog.PostPreviewCategory_GetByCategorySlug", _mockExecuter.CommandText);

        Assert.Equal("cat", _mockExecuter.Parameters?.categorySlug);
    }

    [Fact]
    public async Task GetByCategorySlug_GroupsCategoryRows()
    {
        var row1 = new PostPreviewCategoryDto("x", string.Empty, string.Empty, default, default, default, "cat1", "Thing!", default);
        var row2 = new PostPreviewCategoryDto("x", string.Empty, string.Empty, default, default, default, "cat2", "Thing!", default);
        _mockExecuter.ManyResults = new[] { row1, row2 };

        var results = await _subject.GetByCategorySlug("y");

        Assert.NotNull(results);
        Assert.Equal(1, results.Count);
        Assert.Contains("cat1", results.First().Categories.Select(c => c.Slug));
        Assert.Contains("cat2", results.First().Categories.Select(c => c.Slug));
    }

    [Fact]
    public async Task GetByAuthorId_Executes()
    {
        _mockExecuter.ManyResults = new PostPreviewCategoryDto[]
        {
            new PostPreviewCategoryDto(
                "b",
                "c",
                "d",

                1,
                string.Empty,
                default,

                "cat2",
                "Category 2",

                new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
        };

        _ = await _subject.GetByAuthorId(1);

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("blog.PostPreviewCategory_GetByAuthorId", _mockExecuter.CommandText);

        Assert.Equal(1, _mockExecuter.Parameters?.authorId);
    }

    [Fact]
    public async Task GetByAuthorId_GroupsCategoryRows()
    {
        var row1 = new PostPreviewCategoryDto("x", string.Empty, string.Empty, default, default, default, "cat1", "Thing!", default);
        var row2 = new PostPreviewCategoryDto("x", string.Empty, string.Empty, default, default, default, "cat2", "Thing!", default);
        _mockExecuter.ManyResults = new[] { row1, row2 };

        var results = await _subject.GetByAuthorId(1);

        Assert.NotNull(results);
        Assert.Equal(1, results.Count);
        Assert.Contains("cat1", results.First().Categories.Select(c => c.Slug));
        Assert.Contains("cat2", results.First().Categories.Select(c => c.Slug));
    }
}
