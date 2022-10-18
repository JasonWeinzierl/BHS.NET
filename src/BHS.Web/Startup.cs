using BHS.Web.IoC;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace BHS.Web;

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

        services.AddBhsSwagger();

        services.AddBhsDomain(Configuration)
                .AddBhsInfrastructure();
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

        app.UseBhsSwagger();

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
}
