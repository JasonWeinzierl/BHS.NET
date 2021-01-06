using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace BHS.Web.Controllers.Tests
{
    [Trait("Category", "CompositionRootTest")]
    public class ControllersTests
    {
        private readonly Startup Subject;

        private readonly IServiceCollection _services;

        public ControllersTests()
        {
            var inMemoryData = new Dictionary<string, string>
            {
                { "SENDGRID_API_KEY", "fake api key" }
            };
            var inMemoryConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryData)
                .Build();

            _services = new ServiceCollection();

            Subject = new Startup(inMemoryConfig);
            Subject.ConfigureServices(_services);

            _services.AddSingleton<IConfiguration>(inMemoryConfig);
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
        public void PhotosController_Resolves()
        {
            _services.AddTransient<PhotosController>();
            var serviceProvider = _services.BuildServiceProvider();

            var controller = serviceProvider.GetService<PhotosController>();

            Assert.NotNull(controller);
        }
    }
}
