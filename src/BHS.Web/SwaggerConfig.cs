using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace BHS.Web;

internal static class SwaggerConfig
{
    /// <summary>
    /// Adds the Swagger generator.
    /// </summary>
    public static void AddBhsSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(GetDocumentName(), GetApiInfo());
            opt.IncludeXmlComments(GetXmlCommentsPath(), includeControllerXmlComments: true);

            // Defines a Bearer security scheme.
            // Swagger UI will add the Authorize button to the UI and accept a Bearer token.
            opt.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme.",
                Scheme = JwtBearerDefaults.AuthenticationScheme, // The name to put in the Authorization header before the bearer token.
                BearerFormat = "JWT", // Documentation purposes; indicates how the bearer token is formatted.
            });

            // Selectively applies the Bearer security scheme to endpoints.
            // Swagger UI will send the Bearer token with each request if the scheme is applied.
            // This is more accurate than opt.AddSecurityRequirement() which applies the scheme globally.
            opt.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }

    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Skip endpoints with [AllowAnonymous].
            bool hasAllowAnonymous = context.ApiDescription.CustomAttributes().OfType<AllowAnonymousAttribute>().Any();
            if (hasAllowAnonymous)
                return;

            // Only apply to method endpoints with [Authorize].
            // This is necessary if not applying auth by default with app.MapControllers().RequireAuthorization().
            bool authorize = context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any();
            if (!authorize)
                return;

            // Add possible auth response codes.
            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });

            // Create reference to the scheme definition.
            var schemeReference = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "bearerAuth", // Dictionary key pointing to the definition's name.
                    Type = ReferenceType.SecurityScheme,
                },
            };

            // Add the scheme as a security requirement.
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [schemeReference] = [], // Empty because the array is only used for OAuth2 scopes.
                }
            ];
        }
    }

    private static OpenApiInfo GetApiInfo()
        => new()
        {
            Version = GetDocumentVersion(),
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

    private static string GetDocumentVersion()
        => $"v{Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "next"}";

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
