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
            // TODO: add policies as routes needing authorization are implemented.
            //opt.AddPolicy("WriteAccess", policy => policy.RequireClaim(claimType: "permissions", "blog:write"));
        });

        services.AddControllers()
                .AddBhs400Logging();
        services.AddSpaStaticFiles(opt =>
        {
            opt.RootPath = "ClientApp/dist";
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

        // In production, use the compiled Angular SPA.
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
            spa.Options.SourcePath = "ClientApp";

            // In development, you must start the front end with `npm run start`.
            if (env.IsDevelopment())
            {
                spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/");
            }
        });
    }
}
