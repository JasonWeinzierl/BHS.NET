using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace BHS.Web;

internal static class InvalidModelStateConfig
{
    public static IMvcBuilder ConfigureBhs400Logging(this IMvcBuilder builder)
        => builder.ConfigureApiBehaviorOptions(options =>
        {
            var builtInFactory = options.InvalidModelStateResponseFactory;

            options.InvalidModelStateResponseFactory = context =>
            {
                var response = builtInFactory(context);

                if (response is ObjectResult objectResult && objectResult.Value is HttpValidationProblemDetails problemDetails)
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    using (LogContext.PushProperty("ValidationErrors", problemDetails.Errors))
                    {
                        logger.LogWarning("One or more validation errors occurred.");
                    }
                }

                return response;
            };
        });
}
