using BHS.Domain;
using BHS.Domain.Authors;
using BHS.Domain.Banners;
using BHS.Domain.Blog;
using BHS.Domain.ContactUs;
using BHS.Domain.Leadership;
using BHS.Domain.Photos;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Core.TypeHandlers;
using BHS.Infrastructure.Providers;
using Dapper;
using Microsoft.Data.SqlClient;
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
using System.Data.Common;

namespace BHS.Infrastructure.IoC;

public static class BhsServiceCollectionExtensions
{
    public static IServiceCollection AddBhsServices(this IServiceCollection services)
    {
        services.AddOptions<ContactUsOptions>().BindConfiguration("ContactUsOptions").ValidateDataAnnotations();
        services.AddSendGrid((provider, opt) => provider.GetRequiredService<IConfiguration>().GetSection("SendGridClientOptions").Bind(opt));

        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IPhotosService, PhotosService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IContactUsService, ContactUsService>();
        services.AddScoped<ILeadershipService, LeadershipService>();
        services.AddScoped<ISiteBannerService, SiteBannerService>();

        services.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

        //services.AddMongoRepositories();
        services.AddSqlRepositories();

        return services;
    }

    private static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.DateTime));
        BsonClassMap.RegisterClassMap<Contracts.Author>(map =>
        {
            // Prevent Author.Id from being deserialized as Author._id
            map.AutoMap();
            map.UnmapProperty(c => c.Id);
            map.MapMember(c => c.Id)
                .SetElementName("Id")
                .SetOrder(0)
                .SetIsRequired(true);
        });

        services.TryAddSingleton<IMongoClient>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<MongoClient>>();

            var mongoConnStr = provider.GetRequiredService<IConfiguration>().GetConnectionString("bhsMongo");
            var clientSettings = MongoClientSettings.FromConnectionString(mongoConnStr);
            clientSettings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(e => logger.LogDebug("{CommandName} - {Command}", e.CommandName, e.Command.ToJson()));
            };
            return new MongoClient(clientSettings);
        });

        services.AddSingleton<IPostRepository, Repositories.Mongo.PostRepository>();
        services.AddSingleton<IPostPreviewRepository, Repositories.Mongo.PostPreviewRepository>();
        services.AddSingleton<ICategoryRepository, Repositories.Mongo.CategoryRepository>();
        services.AddSingleton<ILeadershipRepository, Repositories.Mongo.LeadershipRepository>();
        services.AddSingleton<IAlbumRepository, Repositories.Mongo.AlbumRepository>();
        services.AddSingleton<IAuthorRepository, Repositories.Mongo.AuthorRepository>();
        services.AddSingleton<IContactAlertRepository, Repositories.Mongo.ContactAlertRepository>();
        services.AddSingleton<IPhotoRepository, Repositories.Mongo.PhotoRepository>();
        services.AddSingleton<ISiteBannerRepository, Repositories.Mongo.SiteBannerRepository>();

        return services;
    }

    private static IServiceCollection AddSqlRepositories(this IServiceCollection services)
    {
        DbProviderFactories.RegisterFactory(DbConstants.SqlClientProviderName, SqlClientFactory.Instance);
        SqlMapper.AddTypeHandler(DapperUriTypeHandler.Default);

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddSingleton<IDbExecuter, DapperExecuter>();

        // TryAddSingleton so that Mongo repositories can replace them.
        services.TryAddSingleton<IPostRepository, Repositories.Sql.PostRepository>();
        services.TryAddSingleton<IPostPreviewRepository, Repositories.Sql.PostPreviewRepository>();
        services.TryAddSingleton<ICategoryRepository, Repositories.Sql.CategoryRepository>();
        services.TryAddSingleton<IAuthorRepository, Repositories.Sql.AuthorRepository>();
        services.TryAddSingleton<IContactAlertRepository, Repositories.Sql.ContactAlertRepository>();
        services.TryAddSingleton<IPhotoRepository, Repositories.Sql.PhotoRepository>();
        services.TryAddSingleton<IAlbumRepository, Repositories.Sql.AlbumRepository>();
        services.TryAddSingleton<ILeadershipRepository, Repositories.Sql.LeadershipRepository>();
        services.TryAddSingleton<ISiteBannerRepository, Repositories.Sql.SiteBannerRepository>();

        return services;
    }
}
