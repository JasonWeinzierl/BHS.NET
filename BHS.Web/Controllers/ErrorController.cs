using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BHS.Web.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("/api/error-local-development")]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return Problem(
                detail: context?.Error.StackTrace,
                title: context?.Error.Message);
        }

        [HttpGet("/api/error")]
        public IActionResult Error() => Problem();
    }
}
