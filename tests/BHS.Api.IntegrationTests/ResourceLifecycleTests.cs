using BHS.Contracts;
using BHS.Contracts.Blog;
using BHS.Web;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace BHS.Api.IntegrationTests;

[Trait("Category", "Integration")]
[Collection("Sequential")]
public class ResourceLifecycleTests : IClassFixture<BhsWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public ResourceLifecycleTests(BhsWebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task BlogPostLifecycle()
    {
        string slug;

        // CREATE
        var createRequest = new PostRequest("Hello, world!", "# H1 title\n\nThis is a markdown blog post. What's up? 12345", null, null, null, DateTimeOffset.Now, []);
        using var createResponse = await _httpClient.PostAsJsonAsync("/api/blog/posts", createRequest);
        var newPost = await createResponse.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<Post>();

        Assert.NotNull(newPost);
        slug = newPost.Slug;

        // SEARCH
        var searchResults = await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("/api/blog/posts?q=12345");

        Assert.NotNull(searchResults);
        Assert.Contains(slug, searchResults.Select(x => x.Slug));

        // UPDATE
        var updateRequest = new PostRequest("Hello again!", "# title", null, null, new Author("user1", "A User"), createRequest.DatePublished, [new Category("stories", "Stories")]);
        using var updateResponse = await _httpClient.PutAsJsonAsync($"/api/blog/posts/{slug}", updateRequest);
        var updatedPost = await updateResponse.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<Post>();

        Assert.NotNull(updatedPost);

        // GET BY ID
        var postById = await _httpClient.GetFromJsonAsync<Post>($"/api/blog/posts/{slug}");

        Assert.NotNull(postById);

        // DELETE
        using var deleteResponse = await _httpClient.DeleteAsync($"/api/blog/posts/{slug}");
        deleteResponse.EnsureSuccessStatusCode();

        // GET 404
        using var notFoundResponse = await _httpClient.GetAsync($"/api/blog/posts/{slug}");

        Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);

        // GET ALL
        var allPosts = await _httpClient.GetFromJsonAsync<IEnumerable<Post>>("/api/blog/posts");

        Assert.NotNull(allPosts);
        Assert.DoesNotContain(slug, allPosts.Select(x => x.Slug));
    }
}
