using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace BHS.Web;

internal static class SwaggerConfig
{
    private const string DefinitionName = "bearerAuth";

    /// <summary>
    /// Adds the Swagger generator.
    /// </summary>
    public static void AddBhsSwagger(this IServiceCollection services)
    {
        services.AddOpenApi(GetDocumentName(), opt =>
        {
            opt.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                // Specify metadata for the OpenAPI document.
                document.Info = GetApiInfo();
                // XML comments are automatically included.

                // Define a Bearer security scheme.
                // Swagger UI will add the Authorize button to the UI and accept a Bearer token.
                document.Components ??= new();
                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    [DefinitionName] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Scheme = JwtBearerDefaults.AuthenticationScheme, // The name to put in the Authorization header before the bearer token.
                        BearerFormat = "JWT", // Documentation purposes; indicates how the bearer token is formatted.
                    }
                };

                return Task.CompletedTask;
            });

            // Selectively apply the Bearer security scheme to operations.
            // Swagger UI will send the Bearer token with each request, if the scheme is applied.
            opt.AddOperationTransformer<SecurityRequirementsOperationFilter>();
        });
    }

    public class SecurityRequirementsOperationFilter : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            // Skip endpoints with [AllowAnonymous].
            bool hasAllowAnonymous = context.Description.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (hasAllowAnonymous)
                return Task.CompletedTask;

            // Only apply to method endpoints with [Authorize].
            // This is necessary if not applying auth by default with app.MapControllers().RequireAuthorization().
            bool authorize = context.Description.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any();
            if (!authorize)
                return Task.CompletedTask;

            // Add possible auth response codes.
            operation.Responses ??= [];
            operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });

            // Create reference to the security scheme definition.
            var schemeReference = new OpenApiSecuritySchemeReference(
                DefinitionName,
                context.Document);

            // Add the scheme as a security requirement.
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [schemeReference] = [], // Empty because the array is only used for OAuth2 scopes.
                }
            ];
            
            return Task.CompletedTask;
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
                Identifier = "GPL-3.0-or-later",
            },
        };

    private static string GetDocumentName()
        // v1 will result in `openapi.json` so `openapi-ts.config.js` can find it.
        => AssemblyUtil.IsOpenApiGenerator ? "v1" : $"v{AssemblyUtil.MajorVersion}";

    private static string GetDocumentVersion()
        => $"v{AssemblyUtil.SemVer}";

    /// <summary>
    /// Registers middleware for Swagger and SwaggerUI.
    /// </summary>
    public static void UseBhsSwagger(this WebApplication app)
    {
        // Serve the OpenAPI document.
        app.MapOpenApi("api/swagger/{documentName}/swagger.json");

        // Serve the Swagger UI.
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
