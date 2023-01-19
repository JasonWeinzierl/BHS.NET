using BHS.Infrastructure.Repositories.Sql;
using BHS.Infrastructure.Repositories.Sql.Models;
using Xunit;
//using Xunit.Abstractions;

namespace BHS.Infrastructure.Tests.Repositories.Sql;

public class PostRepositoryTests
{
    private readonly PostRepository _subject;

    private readonly MockExecuter _mockExecuter = new();
    //private readonly ITestOutputHelper _output;

    public PostRepositoryTests(/*ITestOutputHelper output*/)
    {
        _subject = new PostRepository(_mockExecuter);
        //_output = output;
    }

    [Fact]
    public async Task GetBySlug_Executes()
    {
        var post = new PostDto(string.Empty, string.Empty, string.Empty, default, default, default, default, default, default, default);
        var category = new CategoryDto(string.Empty, string.Empty);
        _mockExecuter.TwoManyResults = (new[] { post }, new[] { category });

        _ = await _subject.GetBySlug("a");

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("blog.Post_GetBySlug", _mockExecuter.CommandText);

        Assert.Equal("a", _mockExecuter.Parameters?.slug);
    }

    [Fact]
    public async Task GetBySlug_JoinsMultipleResults()
    {
        var post = new PostDto(string.Empty, string.Empty, string.Empty, default, default, default, default, default, default, default);
        var category1 = new CategoryDto(string.Empty, string.Empty);
        var category2 = new CategoryDto(string.Empty, string.Empty);
        _mockExecuter.TwoManyResults = (new[] { post }, new[] { category1, category2 });

        var result = await _subject.GetBySlug("a");

        Assert.NotNull(result);
        Assert.Equal(2, result.Categories.Count);
    }
}
