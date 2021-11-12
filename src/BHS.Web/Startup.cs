using BHS.Web.IoC;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Serilog;
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

            services.LoadApiDomain(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Requests forwarded by reverse proxy.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/api/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/api/error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // In production, use the compiled Angular SPA.
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
                swaggerUIOptions.EnableTryItOutByDefault();
                swaggerUIOptions.DefaultModelsExpandDepth(-1);
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                // In development, you must start the front end with `npm run start`.
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
                }
            });
        }

        private static OpenApiInfo GetApiInfo()
            => new()
            {
                Version = "v1",
                Title = "BHS API",
                Description = "API for the Belton Historical Society",
                TermsOfService = new Uri("/about/terms-of-service", UriKind.Relative),
                Contact = new OpenApiContact
                {
                    Name = "Jason W",
                    Email = "admin@beltonhistoricalsociety.org",
                    Url = new Uri("http://jasonweinzierl.com")
                }
            };

        private static string GetXmlCommentsPath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
    }
}
