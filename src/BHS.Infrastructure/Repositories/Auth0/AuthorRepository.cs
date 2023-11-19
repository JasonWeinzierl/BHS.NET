using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Clients;
using Auth0.ManagementApi.Models;
using BHS.Contracts;
using BHS.Domain.Authors;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;

namespace BHS.Infrastructure.Repositories.Auth0;

public class AuthorRepository : IAuthorRepository
{
    private readonly IUsersClient _usersClient;

    public AuthorRepository(
        IManagementApiClient managementApiClient)
    {
        _usersClient = managementApiClient.Users;
    }

    internal sealed record Auth0Author(string Id, string DisplayName);

    public async Task<IReadOnlyCollection<Author>> GetByAuthUserId(string authUserId, CancellationToken cancellationToken = default)
    {
        var user = await GetAuth0User(authUserId, cancellationToken);

        var authors = ParseAuth0Authors(user)
            ?? throw new FailedAuthorRequestException("Expected Auth0 user's app_metadata.authors to not be null.");

        return authors.Select(a => new Author(a.Id, a.DisplayName)).ToList();
    }

    private async Task<User> GetAuth0User(string authUserId, CancellationToken cancellationToken)
    {
        User user;
        try
        {
            user = await _usersClient.GetAsync(authUserId, cancellationToken: cancellationToken);
        }
        catch (ErrorApiException ex)
        {
            throw ex.StatusCode switch
            {
                HttpStatusCode.BadRequest or HttpStatusCode.NotFound => new InvalidAuthorRequestException($"Auth0 user id {authUserId} is invalid.", ex),
                _ => new FailedAuthorRequestException($"Failed to fetch Auth0 user {authUserId}.", ex),
            };
        }

        return user;
    }

    private static Auth0Author[]? ParseAuth0Authors(User user)
    {
        if (user.AppMetadata is null)
        {
            return [];
        }
        else if (user.AppMetadata is JObject jobject)
        {
            return DeserializeNewtonsoft(jobject);
        }
        else if (user.AppMetadata is JsonElement jsonElement)
        {
            return DeserializeSystemTextJson(jsonElement);
        }
        else
        {
            throw new FailedAuthorRequestException($"Unsupported type for app_metadata of user {user.UserId}: {((object?)user.AppMetadata)?.GetType().FullName ?? "(null)"}");
        }
    }

    private static Auth0Author[]? DeserializeNewtonsoft(JObject appMetadata)
    {
        if (!appMetadata.ContainsKey("authors"))
        {
            return [];
        }
        if (appMetadata["authors"] is not JArray arr)
        {
            throw new FailedAuthorRequestException("Expected app_metadata.authors to be an array.");
        }

        return arr.ToObject<Auth0Author[]>();
    }

    private static readonly JsonSerializerOptions _stjWebOptions = new(JsonSerializerDefaults.Web);
    private static Auth0Author[]? DeserializeSystemTextJson(JsonElement appMetadata)
    {
        if (!appMetadata.TryGetProperty("authors", out var arr))
        {
            return [];
        }

        return arr.Deserialize<Auth0Author[]>(_stjWebOptions);
    }
}
