using BHS.Contracts.Photos;
using BHS.Infrastructure.Repositories;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories;

public class PhotoRepositoryTests
{
    private readonly PhotoRepository _subject;

    private readonly MockExecuter _mockExecuter = new();

    public PhotoRepositoryTests()
    {
        _subject = new PhotoRepository(_mockExecuter);
    }

    [Fact]
    public async Task GetById_Executes()
    {
        _mockExecuter.SingleResult = new Photo(9, "A photo", new Uri("scheme:path"), new DateTimeOffset(2020, 12, 16, 0, 5, 0, TimeSpan.FromHours(-6)), 8, "This is a description.");

        _ = await _subject.GetById(2);

        Assert.Equal(DbConstants.BhsConnectionStringName, _mockExecuter.ConnectionStringName);
        Assert.Equal("photos.Photo_GetById", _mockExecuter.CommandText);

        Assert.Equal(2, _mockExecuter.Parameters?.id);
    }
}
