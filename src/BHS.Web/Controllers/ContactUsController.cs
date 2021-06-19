using BHS.Contracts;
using BHS.Model.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BHS.Web.Controllers
{
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
        [HttpPost]
        public async Task<ActionResult> Post(ContactAlertRequest contactRequest)
        {
            await _contactUsService.AddRequest(contactRequest);
            return Accepted();
        }
    }
}
