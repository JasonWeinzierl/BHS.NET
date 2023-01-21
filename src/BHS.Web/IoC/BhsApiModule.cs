using BHS.Domain;
using BHS.Domain.Authors;
using BHS.Domain.Banners;
using BHS.Domain.Blog;
using BHS.Domain.ContactUs;
using BHS.Domain.Leadership;
using BHS.Domain.Photos;
using BHS.Infrastructure;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Core.TypeHandlers;
using BHS.Infrastructure.Providers;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using SendGrid.Extensions.DependencyInjection;
using System.Data.Common;

namespace BHS.Web.IoC;

public static class BhsApiModule
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

        services.AddSingleton<ILeadershipRepository, Infrastructure.Repositories.Mongo.LeadershipRepository>();
        services.AddSingleton<IAlbumRepository, Infrastructure.Repositories.Mongo.AlbumRepository>();
        services.AddSingleton<IAuthorRepository, Infrastructure.Repositories.Mongo.AuthorRepository>();
        services.AddSingleton<IContactAlertRepository, Infrastructure.Repositories.Mongo.ContactAlertRepository>();
        services.AddSingleton<IPhotoRepository, Infrastructure.Repositories.Mongo.PhotoRepository>();
        services.AddSingleton<ISiteBannerRepository, Infrastructure.Repositories.Mongo.SiteBannerRepository>();

        return services;
    }

    private static IServiceCollection AddSqlRepositories(this IServiceCollection services)
    {
        DbProviderFactories.RegisterFactory(DbConstants.SqlClientProviderName, SqlClientFactory.Instance);
        SqlMapper.AddTypeHandler(DapperUriTypeHandler.Default);

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddSingleton<IDbExecuter, DapperExecuter>();

        // TryAddSingleton so that Mongo repositories can replace them.
        services.TryAddSingleton<IPostRepository, Infrastructure.Repositories.Sql.PostRepository>();
        services.TryAddSingleton<IPostPreviewRepository, Infrastructure.Repositories.Sql.PostPreviewRepository>();
        services.TryAddSingleton<ICategoryRepository, Infrastructure.Repositories.Sql.CategoryRepository>();
        services.TryAddSingleton<IAuthorRepository, Infrastructure.Repositories.Sql.AuthorRepository>();
        services.TryAddSingleton<IContactAlertRepository, Infrastructure.Repositories.Sql.ContactAlertRepository>();
        services.TryAddSingleton<IPhotoRepository, Infrastructure.Repositories.Sql.PhotoRepository>();
        services.TryAddSingleton<IAlbumRepository, Infrastructure.Repositories.Sql.AlbumRepository>();
        services.TryAddSingleton<ILeadershipRepository, Infrastructure.Repositories.Sql.LeadershipRepository>();
        services.TryAddSingleton<ISiteBannerRepository, Infrastructure.Repositories.Sql.SiteBannerRepository>();

        return services;
    }
}
