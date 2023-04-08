using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Contracts.Blog;
using BHS.Contracts.Leadership;
using BHS.Contracts.Photos;
using BHS.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using Xunit;

namespace BHS.Api.IntegrationTests;

[Trait("Category", "Integration")]
[Collection("Sequential")]
public class EndpointTests : IClassFixture<MongoDbWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public EndpointTests(MongoDbWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_Ok()
    {
        using var response = await _httpClient.GetAsync("/api/healthcheck/status");

        string content = await response.Content.ReadAsStringAsync();

        Assert.Equal("Healthy", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Swagger_Ok()
    {
        using var response = await _httpClient.GetAsync("/api/swagger/index.html");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(MediaTypeNames.Text.Html, response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Author_GetAll()
    {
        var authors = await _httpClient.GetFromJsonAsync<IEnumerable<Author>>("/api/author");

        Assert.NotNull(authors);
        Assert.Empty(authors);
    }

    [Fact]
    public async Task Author_GetById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/author/{Random.Shared.Next()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Author_GetPosts_Empty()
    {
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostPreview>>($"/api/author/{Random.Shared.Next()}/posts");

        Assert.NotNull(posts);
        Assert.Empty(posts);
    }

    [Fact]
    public async Task Banners_GetCurrent()
    {
        var banners = await _httpClient.GetFromJsonAsync<IEnumerable<SiteBanner>>("/api/banners/current");

        Assert.NotNull(banners);
        Assert.Empty(banners);
    }

    [Fact]
    public async Task Blog_GetPosts()
    {
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostPreview>>("/api/blog/posts");

        Assert.NotNull(posts);
        Assert.Empty(posts);
    }

    [Fact]
    public async Task Blog_GetPostById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/blog/posts/{Random.Shared.Next()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Blog_GetCategories()
    {
        var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategorySummary>>("/api/blog/categories");

        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task Blog_GetCategoryById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/blog/categories/{Random.Shared.Next()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Leadership_GetOfficers()
    {
        var officers = await _httpClient.GetFromJsonAsync<IEnumerable<Officer>>("/api/leadership/officers");

        Assert.NotNull(officers);
        Assert.Empty(officers);
    }

    [Fact]
    public async Task Leadership_GetDirectors()
    {
        var directors = await _httpClient.GetFromJsonAsync<IEnumerable<Director>>("/api/leadership/directors");

        Assert.NotNull(directors);
        Assert.Empty(directors);
    }

    [Fact]
    public async Task Photos_GetAlbums()
    {
        var albums = await _httpClient.GetFromJsonAsync<IEnumerable<Album>>("/api/photos/albums");

        Assert.NotNull(albums);
        Assert.Empty(albums);
    }

    [Fact]
    public async Task Photos_GetAlbumById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/photos/albums/{Random.Shared.Next()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Photos_GetPhotoById_InvalidId_400()
    {
        string notAnObjectId = Random.Shared.Next().ToString();

        using var response = await _httpClient.GetAsync($"/api/photos/albums/{Random.Shared.Next()}/photos/{notAnObjectId}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Photos_GetPhotoById_UnknownId_404()
    {
        string objectId = ObjectId.GenerateNewId().ToString();

        using var response = await _httpClient.GetAsync($"/api/photos/albums/{Random.Shared.Next()}/photos/{objectId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
