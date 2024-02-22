using BHS.Infrastructure.IoC;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

namespace BHS.Web;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();

        // Set up authentication and read from config section named "Authentication".
        services.AddAuthentication()
                .AddJwtBearer();

        // Set up claim-based authorization.
        services.AddAuthorization(opt =>
        {
            const string permissions = "permissions";
            opt.AddPolicy("BlogWriteAccess", policy => policy.RequireClaim(permissions, "write:blog"));
        });

        services.AddControllers()
                .AddBhs400Logging();
        services.AddSpaStaticFiles(opt =>
        {
            opt.RootPath = "ClientApp/angular";
        });

        services.AddBhsHealthChecks(Configuration);
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
            // In development, you must start the front end with `npm run start`.
            if (env.IsDevelopment())
            {
                spa.Options.SourcePath = "../bhs-web-angular-app";
                spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
            }
        });
    }
}
