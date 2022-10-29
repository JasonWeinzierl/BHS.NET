using BHS.Contracts;
using BHS.Infrastructure.Repositories;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories;

public class AuthorRepositoryTests
{
    private readonly AuthorRepository _subject;

    private readonly MockExecuter _mockExecuter = new();

    public AuthorRepositoryTests()
    {
        _subject = new AuthorRepository(_mockExecuter);
    }

    [Fact]
    public async Task GetByUserName_Executes()
    {
        _mockExecuter.SingleResult = new Author(3, "s", "t");

        _ = await _subject.GetByUserName("b");

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("dbo.Author_GetByUserName", _mockExecuter.CommandText);

        Assert.Equal("b", _mockExecuter.Parameters?.userName);
    }

    [Fact]
    public async Task GetAll_Executes()
    {
        _mockExecuter.ManyResults = new Author[]
        {
            new Author(3, "s", "t")
        };

        _ = await _subject.GetAll();

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("dbo.Author_GetAll", _mockExecuter.CommandText);
    }
}
