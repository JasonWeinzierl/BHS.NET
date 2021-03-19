using BHS.BusinessLogic;
using BHS.BusinessLogic.Blog;
using BHS.BusinessLogic.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Core.TypeHandlers;
using BHS.DataAccess.Repositories;
using BHS.Model.DataAccess;
using BHS.Model.Services;
using BHS.Model.Services.Blog;
using BHS.Model.Services.Photos;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using System.Data.Common;

namespace BHS.Web.IoC
{
    public static class ApiDomainModule
    {
        /// <summary>
        /// Load dependencies for the BHS API.
        /// </summary>
        public static IServiceCollection LoadApiDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSendGrid(options =>
            {
                options.ApiKey = configuration["SENDGRID_API_KEY"];
            });

            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            SqlMapper.AddTypeHandler(DapperUriTypeHandler.Default);

            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IDbExecuter, DapperExecuter>();

            services.AddSingleton<IPostRepository, PostRepository>();
            services.AddSingleton<IPostPreviewRepository, PostPreviewRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IAuthorRepository, AuthorRepository>();
            services.AddSingleton<IContactAlertRepository, ContactAlertRepository>();
            services.AddSingleton<IPhotoRepository, PhotoRepository>();
            services.AddSingleton<IAlbumRepository, AlbumRepository>();

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPhotosService, PhotosService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IContactUsService, ContactUsService>();

            return services;
        }
    }
}
