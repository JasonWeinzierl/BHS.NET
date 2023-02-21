using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BHS.Web;

internal static class SwaggerConfig
{
    /// <summary>
    /// Adds the Swagger generator.
    /// </summary>
    public static void AddBhsSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc(GetDocumentName(), GetApiInfo());

            swaggerGenOptions.IncludeXmlComments(GetXmlCommentsPath(), includeControllerXmlComments: true);
        });
    }

    private static OpenApiInfo GetApiInfo()
        => new()
        {
            Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "vnext",
            Title = "BHS API",
            Description = "API for the Belton Historical Society",
            TermsOfService = new Uri("/about/terms-of-service", UriKind.Relative),
            Contact = new OpenApiContact
            {
                Name = "Jason W",
                Email = "admin@beltonhistoricalsociety.org",
                Url = new Uri("https://jasonweinzierl.com")
            },
            License = new OpenApiLicense
            {
                Name = "GNU General Public License v3.0 or later",
                Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html"),
                // TODO: Add Identifier "GPL-3.0-or-later" when microsoft/OpenAPI.NET#1042 is published.
            },
        };

    private static string GetXmlCommentsPath()
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        return Path.Combine(AppContext.BaseDirectory, xmlFile);
    }

    private static string GetDocumentName()
        => $"v{Assembly.GetExecutingAssembly().GetName().Version?.Major.ToString() ?? "next"}";

    /// <summary>
    /// Registers middleware for Swagger and SwaggerUI.
    /// </summary>
    public static void UseBhsSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(swaggerOptions =>
        {
            swaggerOptions.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(swaggerUIOptions =>
        {
            swaggerUIOptions.DocumentTitle = "BHS API";
            swaggerUIOptions.SwaggerEndpoint($"{GetDocumentName()}/swagger.json", $"BHS API {GetDocumentName()}");
            swaggerUIOptions.RoutePrefix = "api/swagger";
            swaggerUIOptions.EnableTryItOutByDefault();
            swaggerUIOptions.DefaultModelsExpandDepth(-1);
        });
    }
}
