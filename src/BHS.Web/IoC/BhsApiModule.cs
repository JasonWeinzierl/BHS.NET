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
using BHS.Infrastructure.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using SendGrid.Extensions.DependencyInjection;
using System.Data.Common;

namespace BHS.Web.IoC;

public static class BhsApiModule
{
    public static IServiceCollection AddBhsDomain(this IServiceCollection services)
    {
        services.AddOptions<ContactUsOptions>().BindConfiguration("ContactUsOptions").ValidateDataAnnotations();
        services.AddSendGrid((provider, opt) => provider.GetRequiredService<IConfiguration>().GetSection("SendGridClientOptions").Bind(opt));

        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IPhotosService, PhotosService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IContactUsService, ContactUsService>();
        services.AddScoped<ILeadershipService, LeadershipService>();
        services.AddScoped<ISiteBannerService, SiteBannerService>();

        return services;
    }

    public static IServiceCollection AddBhsInfrastructure(this IServiceCollection services)
    {
        DbProviderFactories.RegisterFactory(DbConstants.SqlClientProviderName, SqlClientFactory.Instance);
        SqlMapper.AddTypeHandler(DapperUriTypeHandler.Default);

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddSingleton<IDbExecuter, DapperExecuter>();

        services.AddSingleton<IPostRepository, PostRepository>();
        services.AddSingleton<IPostPreviewRepository, PostPreviewRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<IAuthorRepository, AuthorRepository>();
        services.AddSingleton<IContactAlertRepository, ContactAlertRepository>();
        services.AddSingleton<IPhotoRepository, PhotoRepository>();
        services.AddSingleton<IAlbumRepository, AlbumRepository>();
        services.AddSingleton<ILeadershipRepository, LeadershipRepository>();
        services.AddSingleton<ISiteBannerRepository, SiteBannerRepository>();

        services.AddSingleton<IDateTimeOffsetProvider, DateTimeOffsetProvider>();

        return services;
    }
}
