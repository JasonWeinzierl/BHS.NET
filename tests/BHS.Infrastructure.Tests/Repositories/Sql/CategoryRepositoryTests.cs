using BHS.Contracts.Blog;
using BHS.Infrastructure.Models;
using BHS.Infrastructure.Repositories.Sql;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories.Sql;

public class CategoryRepositoryTests
{
    private readonly CategoryRepository _subject;

    private readonly MockExecuter _mockExecuter = new();

    public CategoryRepositoryTests()
    {
        _subject = new CategoryRepository(_mockExecuter);
    }

    [Fact]
    public async Task GetBySlug_Executes()
    {
        _mockExecuter.SingleResult = new Category("thing", "Thing!");

        _ = await _subject.GetBySlug("y");

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("blog.Category_GetBySlug", _mockExecuter.CommandText);

        Assert.Equal("y", _mockExecuter.Parameters?.slug);
    }

    [Fact]
    public async Task GetAll_Executes()
    {
        _mockExecuter.ManyResults = new CategorySummaryDto[]
        {
            new CategorySummaryDto("thing", "Thing!", 0)
        };

        _ = await _subject.GetAll();

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("blog.CategorySummary_GetAll", _mockExecuter.CommandText);
    }
}
