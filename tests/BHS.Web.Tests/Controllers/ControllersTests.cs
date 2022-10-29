using BHS.Web.Controllers;
using BHS.Web.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BHS.Web.Tests.Controllers;

[Trait("Category", "CompositionRootTest")]
public class ControllersTests
{
    private readonly IServiceCollection _services;

    public ControllersTests()
    {
        var inMemoryConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "SendGridClientOptions:ApiKey", "fake api key" }
            })
            .Build();

        _services = new ServiceCollection()
                .AddBhsDomain()
                .AddBhsInfrastructure();
        
        _services.AddSingleton<IConfiguration>(inMemoryConfig);
        _services.AddLogging();
        _services.AddHealthChecks();
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
        _services.AddTransient(controllerType);
        using var serviceProvider = _services.BuildServiceProvider();

        var controller = serviceProvider.GetService(controllerType);

        Assert.NotNull(controller);
    }
}
