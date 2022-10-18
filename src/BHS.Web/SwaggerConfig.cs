using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BHS.Web;

internal static class SwaggerConfig
{
    public static void AddBhsSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1", GetApiInfo());

            swaggerGenOptions.IncludeXmlComments(GetXmlCommentsPath(), includeControllerXmlComments: true);
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
                Url = new Uri("https://jasonweinzierl.com")
            }
        };

    private static string GetXmlCommentsPath()
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        return Path.Combine(AppContext.BaseDirectory, xmlFile);
    }

    public static void UseBhsSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(swaggerOptions =>
        {
            swaggerOptions.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(swaggerUIOptions =>
        {
            swaggerUIOptions.DocumentTitle = "BHS API";
            swaggerUIOptions.SwaggerEndpoint("v1/swagger.json", "BHS API V1");
            swaggerUIOptions.RoutePrefix = "api/swagger";
            swaggerUIOptions.EnableTryItOutByDefault();
            swaggerUIOptions.DefaultModelsExpandDepth(-1);
        });
    }
}
