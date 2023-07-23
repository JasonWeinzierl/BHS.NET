using Auth0.ManagementApi;
using Auth0.ManagementApi.Clients;
using Auth0.ManagementApi.Models;
using BHS.Infrastructure.Repositories.Auth0;
using Moq;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories;

public class AuthorRepositoryTests
{
    private readonly Mock<IUsersClient> _mockUsersClient = new(MockBehavior.Strict);
    private readonly Mock<IManagementApiClient> _mockManagementApiClient = new(MockBehavior.Strict);

    private AuthorRepository Subject => new(_mockManagementApiClient.Object);

    public AuthorRepositoryTests()
    {
        _mockManagementApiClient.SetupGet(c => c.Users).Returns(_mockUsersClient.Object);
    }

    [Fact]
    public async Task HappyPath_Newtonsoft()
    {
        // Arrange
        string userId = "12345";

        var user = new User
        {
            AppMetadata = JObject.FromObject(new
            {
                authors = new[]
                {
                    new { id = "test", displayName = "Test" },
                },
            }),
        };
        _mockUsersClient
            .Setup(c => c.GetAsync(userId, null, true, default))
            .ReturnsAsync(user);

        // Act
        var authors = await Subject.GetByAuthUserId(userId);

        // Assert
        var author = Assert.Single(authors);
        Assert.Equal("test", author.Username);
        Assert.Equal("Test", author.Name);
    }

    [Fact]
    public async Task HappyPath_SystemTextJson()
    {
        // Arrange
        string userId = "12345";

        var user = new User
        {
            AppMetadata = JsonSerializer.SerializeToElement(new
            {
                authors = new[]
                {
                    new { id = "test", displayName = "Test" },
                },
            }),
        };
        _mockUsersClient
            .Setup(c => c.GetAsync(userId, null, true, default))
            .ReturnsAsync(user);

        // Act
        var authors = await Subject.GetByAuthUserId(userId);

        // Assert
        var author = Assert.Single(authors);
        Assert.Equal("test", author.Username);
        Assert.Equal("Test", author.Name);
    }
}
