using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace BHS.Web;

internal static class InvalidModelStateConfig
{
    /// <summary>
    /// Adds logging when automatic 400 responses occur.
    /// </summary>
    /// <remarks>
    /// <see cref="ApiControllerAttribute"/> makes model validation errors automatically trigger an HTTP 400 response.
    /// This method ensures those errors are logged while otherwise preserving the same response behavior.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/aspnet/core/web-api#log-automatic-400-responses" />
    public static IMvcBuilder AddBhs400Logging(this IMvcBuilder builder)
        => builder.ConfigureApiBehaviorOptions(options =>
        {
            var builtInFactory = options.InvalidModelStateResponseFactory;

            options.InvalidModelStateResponseFactory = context =>
            {
                var response = builtInFactory(context);

                if (response is ObjectResult objectResult && objectResult.Value is HttpValidationProblemDetails problemDetails)
                {
                    var controllerType = context.HttpContext.GetEndpoint()
                                                            ?.Metadata.GetMetadata<ControllerActionDescriptor>()
                                                            ?.ControllerTypeInfo.AsType()
                                                            ?? typeof(Program);

                    var logger = context.HttpContext.RequestServices
                                        .GetRequiredService<ILoggerFactory>()
                                        .CreateLogger(controllerType);

                    if (logger.IsEnabled(LogLevel.Warning))
                    {
                        logger.LogWarning("One or more validation errors occurred: {ValidationErrors}", problemDetails.Errors);
                    }
                }

                return response;
            };
        });
}
