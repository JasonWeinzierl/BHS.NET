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
        /// Loads services in the BHS API domain.
        /// </summary>
        /// <returns> A reference to this instance after the operation has completed. </returns>
        public static IServiceCollection LoadApiDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ContactUsOptions>(opt => configuration.GetSection(nameof(ContactUsOptions)).Bind(opt));

            services.AddSendGrid(opt => configuration.GetSection("SendGridClientOptions").Bind(opt));

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

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPhotosService, PhotosService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<ILeadershipService, LeadershipService>();
            services.AddScoped<ISiteBannerService, SiteBannerService>();

            return services;
        }
    }
}
