using BHS.Contracts;
using BHS.Model.DataAccess;
using BHS.Model.Services;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace BHS.BusinessLogic
{
    public class ContactUsService : IContactUsService
    {
        private readonly IContactAlertRepository _contactAlertRepository;
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger _logger;

        public ContactUsService(
            IContactAlertRepository contactAlertRepository,
            ISendGridClient sendGridClient,
            ILogger<ContactUsService> logger)
        {
            _contactAlertRepository = contactAlertRepository;
            _sendGridClient = sendGridClient;
            _logger = logger;
        }

        public async Task AddRequest(ContactAlertRequest request)
        {
            //return;
            // Body is a honeypot.
            if (!string.IsNullOrEmpty(request.Body))
                return;

            var newAlert = await _contactAlertRepository.Insert(request);

            var submitTime = newAlert.DateRequested ?? newAlert.DateCreated;
            // Set to CST, but avoid an ArgumentOutOfRangeException.
            if (submitTime != default)
                submitTime = submitTime.ToOffset(TimeSpan.FromHours(-6));

            var msg = new SendGridMessage
            {
                From = new EmailAddress("contact@beltonhistoricalsociety.org", "Belton Historical Society"),
                Subject = "Website Comment: " + newAlert.Name
            };
            msg.AddContent(MimeType.Html, @$"
<p>The following form has been submitted from your website <a>https://www.beltonhistoricalsociety.org/</a> :</p>

<p>
Name: {newAlert.Name}<br>
Email Address: {newAlert.EmailAddress}<br>
Message:<br>
{newAlert.Message}
</p>

<p><i>Submitted on {submitTime:dddd, d MMMM yyyy HH:mm:ss zzz} .</i></p>");
            msg.AddTo("weinzierljason@gmail.com");

            var response = await _sendGridClient.SendEmailAsync(msg);

            _logger.LogInformation("CreatedContactAlert {0}: {1}", newAlert.Id, response.StatusCode);
        }
    }
}
