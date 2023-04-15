using BHS.Domain.ContactUs;
using BHS.Domain.Photos;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BHS.Web.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/api/error-local-development")]
    public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");

        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        return Problem(
            detail: context?.Error.StackTrace,
            statusCode: GetStatusCode(context?.Error),
            title: context?.Error.Message);
    }

    [Route("/api/error")]
    public IActionResult Error()
        => Problem(statusCode: GetStatusCode(HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error));

    private static int GetStatusCode(Exception? exception) =>
        exception switch
        {
            InvalidContactRequestException => StatusCodes.Status400BadRequest,
            InvalidPhotoIdException => StatusCodes.Status400BadRequest,

            MongoWriteException ex when ex.WriteError.Category == ServerErrorCategory.DuplicateKey => StatusCodes.Status409Conflict,

            NotImplementedException => StatusCodes.Status501NotImplemented,

            _ => StatusCodes.Status500InternalServerError,
        };
}
