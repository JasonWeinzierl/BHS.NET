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
            var inMemoryData = new Dictionary<string, string>
            {
                { "SendGridClientOptions:ApiKey", "fake api key" }
            };
            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryData)
                .Build();

            _services = new ServiceCollection();
            _services.AddSingleton<IConfiguration>(inMemoryConfig);
            _services.LoadApiDomain(inMemoryConfig);
        }

        [Fact]
        public void AuthorController_Resolves()
        {
            _services.AddTransient<AuthorController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<AuthorController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void BlogController_Resolves()
        {
            _services.AddTransient<BlogController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<BlogController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void ContactUsController_Resolves()
        {
            _services.AddTransient<ContactUsController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<ContactUsController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void ErrorController_Resolves()
        {
            _services.AddTransient<ErrorController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<ErrorController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void LeadershipController_Resolves()
        {
            _services.AddTransient<LeadershipController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<LeadershipController>();

            Assert.NotNull(controller);
        }

        [Fact]
        public void PhotosController_Resolves()
        {
            _services.AddTransient<PhotosController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<PhotosController>();

            Assert.NotNull(controller);
        }
    }
}
