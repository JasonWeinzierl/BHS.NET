using BHS.BusinessLogic;
using BHS.BusinessLogic.Blog;
using BHS.BusinessLogic.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace BHS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", GetApiInfo());

                swaggerGenOptions.IncludeXmlComments(GetXmlCommentsPath());
            });

            services.AddSendGrid(options =>
            {
                options.ApiKey = Configuration["SENDGRID_API_KEY"];
            });

            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

            #region BHS Dependencies

            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IQuerier, Querier>();

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

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/api/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/api/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseSwagger(swaggerOptions =>
            {
                swaggerOptions.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(swaggerUIOptions =>
            {
                swaggerUIOptions.SwaggerEndpoint("v1/swagger.json", "BHS API V1");
                swaggerUIOptions.RoutePrefix = "api/swagger";
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), configuration =>
            {
                configuration.UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "ClientApp";

                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                });
            });
        }

        private static OpenApiInfo GetApiInfo()
        {
            return new OpenApiInfo
            {
                Version = "v1",
                Title = "BHS API",
                Description = "API for the Belton Historical Society",
                TermsOfService = new Uri("/termsofservice", UriKind.Relative),
                Contact = new OpenApiContact
                {
                    Name = "Jason W",
                    Email = string.Empty,
                    Url = new Uri("http://jasonweinzierl.com")
                }
            };
        }

        private static string GetXmlCommentsPath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
    }
}
