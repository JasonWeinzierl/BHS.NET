using Auth0.ManagementApi;
using Auth0.ManagementApi.Clients;
using Auth0.ManagementApi.Models;
using BHS.Infrastructure.Repositories.Auth0;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System.Text.Json;
using Xunit;

namespace BHS.Infrastructure.Tests.Repositories;

public class AuthorRepositoryTests
{
    private readonly IUsersClient _usersClient = Substitute.For<IUsersClient>();
    private readonly IManagementApiClient _managementApiClient = Substitute.For<IManagementApiClient>();

    private AuthorRepository Subject => new(_managementApiClient);

    public AuthorRepositoryTests()
    {
        _managementApiClient.Users.Returns(_usersClient);
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
        _usersClient
            .GetAsync(userId, null, true, default)
            .Returns(user);

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
        _usersClient
            .GetAsync(userId, null, true, default)
            .Returns(user);

        // Act
        var authors = await Subject.GetByAuthUserId(userId);

        // Assert
        var author = Assert.Single(authors);
        Assert.Equal("test", author.Username);
        Assert.Equal("Test", author.Name);
    }
}
