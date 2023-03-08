using BHS.Domain;
using BHS.Domain.Authors;
using BHS.Domain.Banners;
using BHS.Domain.Blog;
using BHS.Domain.ContactUs;
using BHS.Domain.Leadership;
using BHS.Domain.Notifications;
using BHS.Domain.Photos;
using BHS.Infrastructure.Adapters;
using BHS.Infrastructure.Providers;
using BHS.Infrastructure.Repositories.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using SendGrid.Extensions.DependencyInjection;

namespace BHS.Infrastructure.IoC;

public static class BhsServiceCollectionExtensions
{
    public static IServiceCollection AddBhsServices(this IServiceCollection services)
    {
        services.AddOptions<ContactUsOptions>().BindConfiguration("ContactUsOptions").ValidateDataAnnotations();
        services.AddOptions<NotificationOptions>().BindConfiguration("NotificationOptions").ValidateDataAnnotations();
        services.AddSendGrid((provider, opt) => provider.GetRequiredService<IConfiguration>().GetSection("SendGridClientOptions").Bind(opt));

        services.AddScoped<IContactUsService, ContactUsService>();
        services.TryAddScoped<IEmailAdapter, SendGridEmailAdapter>();

        services.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

        services.AddMongoRepositories();

        return services;
    }

    private static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.DateTime));

        services.TryAddSingleton<IMongoClient>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<MongoClient>>();

            var mongoConnStr = provider.GetRequiredService<IConfiguration>().GetConnectionString("bhsMongo");
            var clientSettings = MongoClientSettings.FromConnectionString(mongoConnStr);
            clientSettings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(e => logger.LogDebug("{CommandName} - {Command}", e.CommandName, e.Command.ToJson()));
            };
            clientSettings.LinqProvider = MongoDB.Driver.Linq.LinqProvider.V2; // TODO: V3 causes errors; investigate and resolve. https://github.com/mongodb/mongo-csharp-analyzer/blob/main/tests/MongoDB.Analyzer.Tests.Common.TestCases/Linq/NotSupportedLinq2.cs
            return new MongoClient(clientSettings);
        });

        services.AddSingleton<IPostRepository, PostRepository>();
        services.AddSingleton<IPostPreviewRepository, PostPreviewRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<ILeadershipRepository, LeadershipRepository>();
        services.AddSingleton<IAlbumRepository, AlbumRepository>();
        services.AddSingleton<IAuthorRepository, AuthorRepository>();
        services.AddSingleton<IContactAlertRepository, ContactAlertRepository>();
        services.AddSingleton<ISiteBannerRepository, SiteBannerRepository>();

        return services;
    }
}
