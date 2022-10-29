using BHS.Contracts;
using BHS.Domain.ContactUs;
using Microsoft.AspNetCore.Mvc;

namespace BHS.Web.Controllers;

[ApiController]
[Route("api/contact-us")]
public class ContactUsController : ControllerBase
{
    private readonly IContactUsService _contactUsService;

    public ContactUsController(IContactUsService contactUsService)
    {
        _contactUsService = contactUsService;
    }

    /// <summary>
    /// Add a request for being contacted.
    /// </summary>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult> Post(ContactAlertRequest contactRequest)
    {
        _ = await _contactUsService.AddRequest(contactRequest);
        return Accepted();
    }
}
