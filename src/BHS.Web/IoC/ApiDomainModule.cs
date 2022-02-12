using BHS.BusinessLogic;
using BHS.BusinessLogic.Banners;
using BHS.BusinessLogic.Blog;
using BHS.BusinessLogic.Leadership;
using BHS.BusinessLogic.Photos;
using BHS.DataAccess;
using BHS.DataAccess.Core;
using BHS.DataAccess.Core.TypeHandlers;
using BHS.DataAccess.Providers;
using BHS.DataAccess.Repositories;
using BHS.Domain;
using BHS.Domain.Providers;
using BHS.Domain.Repositories;
using BHS.Domain.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using SendGrid.Extensions.DependencyInjection;
using System.Data.Common;

namespace BHS.Web.IoC
{
    public static class ApiDomainModule
    {
        /// <summary>
        /// Adds the required services for the BHS Web API.
        /// </summary>
        /// <returns> The <see cref="IServiceCollection"/>. </returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            AddDataAccess(services);
            AddBusinessLogic(services, configuration);

            return services;
        }

        private static void AddDataAccess(IServiceCollection services)
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
        }

        private static void AddBusinessLogic(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ContactUsOptions>(opt => configuration.GetSection("ContactUsOptions").Bind(opt));
            services.AddSendGrid(opt => configuration.GetSection("SendGridClientOptions").Bind(opt));

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPhotosService, PhotosService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<ILeadershipService, LeadershipService>();
            services.AddScoped<ISiteBannerService, SiteBannerService>();
        }
    }
}
