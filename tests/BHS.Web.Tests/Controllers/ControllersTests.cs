using BHS.Infrastructure.IoC;
using BHS.Web.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Xunit;

namespace BHS.Web.Tests.Controllers;

public sealed class ControllerTestsClassFixture
{
    public ControllerTestsClassFixture()
    {
        var inMemoryConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "SendGridClientOptions:ApiKey", "fake api key" },

                { "ContactUsOptions:ToAddresses:0", "test@test.com" },

                { "NotificationOptions:FromAddress", "test@test.com" },
                { "NotificationOptions:FromName", "test" },
            })
            .Build();

        Services = new ServiceCollection();

        // Mock any services which shouldn't be instantiated.
        Services.AddSingleton(Moq.Mock.Of<IMongoClient>());

        // Subject under test.
        Services.AddBhsServices();

        // Add other services provided by the Generic Host.
        Services.AddSingleton<IConfiguration>(inMemoryConfig);
        Services.AddLogging();
        Services.AddHealthChecks();
    }

    public IServiceCollection Services { get; }
}

[Trait("Category", "CompositionRootTest")]
public class ControllersTests : IClassFixture<ControllerTestsClassFixture>
{
    private readonly ControllerTestsClassFixture _fixture;

    public ControllersTests(ControllerTestsClassFixture fixture)
    {
        _fixture = fixture;
    }

    public static IEnumerable<object[]> Controllers => new[]
    {
        new[] { typeof(AuthorController) },
        new[] { typeof(BannersController) },
        new[] { typeof(BlogController) },
        new[] { typeof(ContactUsController) },
        new[] { typeof(ErrorController) },
        new[] { typeof(HealthCheckController) },
        new[] { typeof(LeadershipController) },
        new[] { typeof(PhotosController) },
    };

    [Theory]
    [MemberData(nameof(Controllers))]
    public void Controller_Resolves(Type controllerType)
    {
        _fixture.Services.AddTransient(controllerType);
        using var serviceProvider = _fixture.Services.BuildServiceProvider();

        var controller = serviceProvider.GetService(controllerType);

        Assert.NotNull(controller);
    }
}
