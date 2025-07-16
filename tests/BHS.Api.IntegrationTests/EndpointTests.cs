using BHS.Contracts;
using BHS.Contracts.Banners;
using BHS.Contracts.Blog;
using BHS.Contracts.Leadership;
using BHS.Contracts.Photos;
using BHS.Web;
using MongoDB.Bson;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using Xunit;

namespace BHS.Api.IntegrationTests;

[Trait("Category", "Integration")]
[Collection("Sequential")]
public class EndpointTests(BhsWebApplicationFactory<Program> factory) : IClassFixture<BhsWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task HealthCheck_Ok()
    {
        using var response = await _httpClient.GetAsync("/api/healthcheck/status", TestContext.Current.CancellationToken);

        string content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal("Healthy", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Swagger_Ok()
    {
        using var response = await _httpClient.GetAsync("/api/swagger/index.html", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(MediaTypeNames.Text.Html, response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Author_GetByAuthUserId_Empty()
    {
        using var response = await _httpClient.GetAsync("/api/authors?authUserId=12345", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(MediaTypeNames.Application.Json, response.Content.Headers.ContentType?.MediaType);

        var authors = await response.Content.ReadFromJsonAsync<IEnumerable<Author>>(TestContext.Current.CancellationToken);
        Assert.NotNull(authors);
        Assert.Empty(authors);
    }

    [Fact]
    public async Task Author_GetPosts_Empty()
    {
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostPreview>>($"/api/authors/{Random.Shared.Next()}/posts", TestContext.Current.CancellationToken);

        Assert.NotNull(posts);
        Assert.Empty(posts);
    }

    [Fact]
    public async Task Banners_GetCurrent()
    {
        var banners = await _httpClient.GetFromJsonAsync<IEnumerable<SiteBanner>>("/api/banners/current", TestContext.Current.CancellationToken);

        Assert.NotNull(banners);
    }

    [Fact]
    public async Task Banners_GetHistory()
    {
        var banners = await _httpClient.GetFromJsonAsync<IEnumerable<SiteBannerHistory>>("/api/banners/history", TestContext.Current.CancellationToken);

        Assert.NotNull(banners);
    }

    [Fact]
    public async Task Banners_Insert()
    {
        var request = new SiteBannerRequest(AlertTheme.Success, "Hello", "world");

        using var response = await _httpClient.PostAsJsonAsync("/api/banners", request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var banner = await response.Content.ReadFromJsonAsync<SiteBanner>(TestContext.Current.CancellationToken);

        Assert.NotNull(banner);
        Assert.Equal(AlertTheme.Success, banner.Theme);
        Assert.Equal("Hello", banner.Lead);
        Assert.Equal("world", banner.Body);
    }

    [Fact]
    public async Task Banners_Delete()
    {
        var request = new SiteBannerRequest(AlertTheme.Success, "Hello", "world");

        using var createResponse = await _httpClient.PostAsJsonAsync("/api/banners", request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);
        var banner = await createResponse.Content.ReadFromJsonAsync<SiteBanner>(TestContext.Current.CancellationToken);

        Assert.NotNull(banner);
        Assert.Equal(AlertTheme.Success, banner.Theme);
        Assert.Equal("Hello", banner.Lead);
        Assert.Equal("world", banner.Body);

        using var deleteResponse = await _httpClient.DeleteAsync($"/api/banners/{banner.Id}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Blog_GetPosts()
    {
        var posts = await _httpClient.GetFromJsonAsync<IEnumerable<PostPreview>>($"/api/blog/posts?q={Random.Shared.Next()}", TestContext.Current.CancellationToken);

        Assert.NotNull(posts);
        Assert.Empty(posts);
    }

    [Fact]
    public async Task Blog_CreatePost()
    {
        var datePublished = new DateTimeOffset(2023, 04, 19, 20, 20, 00, 999, TimeSpan.FromHours(-5));
        var request = new PostRequest(
            "Hello, world!",
            "# Title",
            null,
            null,
            new Author("me", "me :)"),
            datePublished,
            [new Category("newsletters", "Newsletters")]);

        using var response = await _httpClient.PostAsJsonAsync("/api/blog/posts", request, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var post = await response.Content.ReadFromJsonAsync<Post>(TestContext.Current.CancellationToken);

        Assert.NotNull(post);
        Assert.Contains("-hello-world", post.Slug);
        Assert.Equal(request.Title, post.Title);
        Assert.Equal(request.ContentMarkdown, post.ContentMarkdown);
        Assert.Null(post.FilePath);
        Assert.Null(post.PhotosAlbumSlug);
        Assert.Equal(request.Author, post.Author);
        Assert.Equal(datePublished, post.DatePublished);
        Assert.NotEqual(default, post.DateLastModified);
        var category = Assert.Single(post.Categories);
        Assert.Equal(request.Categories.Single().Slug, category.Slug);
        Assert.Equal(request.Categories.Single().Name, category.Name);
    }

    [Fact]
    public async Task Blog_UpdatePost()
    {
        var createRequest = new PostRequest(
            "A post!",
            "# First revision",
            null,
            null,
            new Author("me", "me :)"),
            DateTimeOffset.Now,
            [new Category("cat1", "Cat1"), new Category("cat2", "Cat2")]);
        var updateRequest = createRequest with
        {
            ContentMarkdown = "# Second revision",
            DatePublished = new DateTimeOffset(2000, 01, 01, 00, 00, 00, 000, TimeSpan.FromHours(0)),
            Categories = [new Category("cat1", "Cat1"), new Category("cat3", "Cat3")],
        };

        using var createResponse = await _httpClient.PostAsJsonAsync("/api/blog/posts", createRequest, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var initialPost = await createResponse.Content.ReadFromJsonAsync<Post>(TestContext.Current.CancellationToken);
        Assert.NotNull(initialPost?.Slug);

        using var response2 = await _httpClient.PutAsJsonAsync($"/api/blog/posts/{initialPost.Slug}", updateRequest, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var updatedPost = await response2.Content.ReadFromJsonAsync<Post>(TestContext.Current.CancellationToken);

        Assert.NotNull(updatedPost);
        Assert.Equal(initialPost.Slug, updatedPost.Slug);
        Assert.Equal(updateRequest.ContentMarkdown, updatedPost.ContentMarkdown);
        Assert.Equal(updateRequest.DatePublished, updatedPost.DatePublished);
        Assert.NotEqual(initialPost.DateLastModified, updatedPost.DateLastModified);
        Assert.Collection(updatedPost.Categories, item1 => Assert.Equal("cat1", item1.Slug), item2 => Assert.Equal("cat3", item2.Slug));
    }

    [Fact]
    public async Task Blog_DeletePost()
    {
        var createRequest = new PostRequest(
            "Doomed",
            "# Title",
            null,
            null,
            null,
            DateTimeOffset.Now,
            []);

        using var createResponse = await _httpClient.PostAsJsonAsync("/api/blog/posts", createRequest, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var post = await createResponse.Content.ReadFromJsonAsync<Post>(TestContext.Current.CancellationToken);
        Assert.NotNull(post);

        using var deleteResponse = await _httpClient.DeleteAsync($"/api/blog/posts/{post.Slug}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task Blog_GetPostById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/blog/posts/{Random.Shared.Next()}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Blog_GetCategories()
    {
        var categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategorySummary>>("/api/blog/categories", TestContext.Current.CancellationToken);

        Assert.NotNull(categories);
    }

    [Fact]
    public async Task Blog_GetCategoryById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/blog/categories/{Random.Shared.Next()}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Leadership_GetOfficers()
    {
        var officers = await _httpClient.GetFromJsonAsync<IEnumerable<Officer>>("/api/leadership/officers", TestContext.Current.CancellationToken);

        Assert.NotNull(officers);
        Assert.Empty(officers);
    }

    [Fact]
    public async Task Leadership_GetDirectors()
    {
        var directors = await _httpClient.GetFromJsonAsync<IEnumerable<Director>>("/api/leadership/directors", TestContext.Current.CancellationToken);

        Assert.NotNull(directors);
        Assert.Empty(directors);
    }

    [Fact]
    public async Task Photos_GetAlbums()
    {
        var albums = await _httpClient.GetFromJsonAsync<IEnumerable<Album>>("/api/photos/albums", TestContext.Current.CancellationToken);

        Assert.NotNull(albums);
        Assert.Empty(albums);
    }

    [Fact]
    public async Task Photos_GetAlbumById_404()
    {
        using var response = await _httpClient.GetAsync($"/api/photos/albums/{Random.Shared.Next()}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Photos_GetPhotoById_InvalidId_400()
    {
        string notAnObjectId = Random.Shared.Next().ToString();

        using var response = await _httpClient.GetAsync($"/api/photos/albums/{Random.Shared.Next()}/photos/{notAnObjectId}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Photos_GetPhotoById_UnknownId_404()
    {
        string objectId = ObjectId.GenerateNewId().ToString();

        using var response = await _httpClient.GetAsync($"/api/photos/albums/{Random.Shared.Next()}/photos/{objectId}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
