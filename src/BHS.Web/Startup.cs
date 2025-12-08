using BHS.Infrastructure.IoC;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace BHS.Web;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();

        services.AddBhsAuth();

        services.AddControllers()
                .AddBhs400Logging();
        services.AddSpaStaticFiles(opt =>
        {
            opt.RootPath = "ClientApp/angular";
        });

        services.AddBhsHealthChecks();
        services.AddBhsSwagger();

        services.AddBhsServices();
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

        // In release builds, use the compiled Angular SPA.
        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseBhsSwagger();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSpa(spa =>
        {
            // In development, you must start the front end with `yarn start`.
            if (env.IsDevelopment())
            {
                spa.Options.SourcePath = "../bhs-web-angular-app";
                spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
            }
        });
    }
}
