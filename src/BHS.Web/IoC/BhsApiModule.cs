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
using MongoDB.Driver;
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
        services.TryAddSingleton<IMongoClient>(provider =>
        {
            var mongoConnStr = provider.GetRequiredService<IConfiguration>().GetConnectionString("bhsMongo");
            return new MongoClient(mongoConnStr);
        });

        services.AddScoped<ILeadershipRepository, Infrastructure.Repositories.Mongo.LeadershipRepository>();

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
