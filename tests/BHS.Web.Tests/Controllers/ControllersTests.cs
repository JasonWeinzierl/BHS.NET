using BHS.Web.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BHS.Web.Controllers.Tests
{
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
                    .AddSingleton<IConfiguration>(inMemoryConfig)
                    .AddDomainServices(inMemoryConfig);
        }

        [Fact]
        public void AuthorController_Resolves()
        {
            _services.AddTransient<AuthorController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<AuthorController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void BannersController_Resolves()
        {
            _services.AddTransient<BannersController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<BannersController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void BlogController_Resolves()
        {
            _services.AddTransient<BlogController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<BlogController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void ContactUsController_Resolves()
        {
            _services.AddTransient<ContactUsController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<ContactUsController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void ErrorController_Resolves()
        {
            _services.AddTransient<ErrorController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<ErrorController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void LeadershipController_Resolves()
        {
            _services.AddTransient<LeadershipController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<LeadershipController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void PhotosController_Resolves()
        {
            _services.AddTransient<PhotosController>();
            using var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<PhotosController>();

            Assert.NotNull(controller);
        }
    }
}
