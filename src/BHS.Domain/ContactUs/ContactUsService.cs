﻿using BHS.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BHS.Domain.ContactUs
{
    public class ContactUsService : IContactUsService
    {
        private readonly ContactUsOptions _options;
        private readonly IContactAlertRepository _contactAlertRepository;
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger _logger;

        public ContactUsService(
            IOptions<ContactUsOptions> options,
            IContactAlertRepository contactAlertRepository,
            ISendGridClient sendGridClient,
            ILogger<ContactUsService> logger)
        {
            _options = options.Value;
            _contactAlertRepository = contactAlertRepository;
            _sendGridClient = sendGridClient;
            _logger = logger;
        }

        public async Task<ContactAlert?> AddRequest(ContactAlertRequest request)
        {
            if (FailsHoneypotCheck(request))
            {
                _logger.LogInformation("ContactAlertRequest failed honeypot check.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(request.EmailAddress))
                throw new BadRequestException("Email address is required.");

            var newAlert = await _contactAlertRepository.Insert(request);
            await SendEmail(newAlert);

            return newAlert;
        }

        private static bool FailsHoneypotCheck(ContactAlertRequest request) => !string.IsNullOrEmpty(request.Body);

        private async Task SendEmail(ContactAlert newAlert)
        {
            // Windows Ids are supported on Linux as of .NET 6 #49412.
            var cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var submitTime = GetSubmitTime(newAlert, cst);

            var msg = new SendGridMessage
            {
                From = new EmailAddress(_options.FromAddress, _options.FromName),
                Subject = "Website Comment: " + newAlert.Name
            };
            msg.AddContent(MimeType.Text, newAlert.Message);
            msg.AddContent(MimeType.Html, @$"
<p>The following message has been submitted to the Belton Historical Society:</p>

<p>
Name: {newAlert.Name}<br>
Email Address: {newAlert.EmailAddress}<br>
Message:<br>
{newAlert.Message}
</p>

<p><i>Submitted on {submitTime:dddd, d MMMM yyyy HH:mm:ss zzz} .</i></p>");
            foreach (var toAddress in _options.ToAddresses)
                msg.AddTo(toAddress);

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("CreatedContactAlert #{Id}: {StatusCode} {Status}", newAlert.Id, (int)response.StatusCode, response.StatusCode);
            }
            else
            {
                string errorString = await response.Body.ReadAsStringAsync();
                _logger.LogError("FailedContactAlert #{Id}: {StatusCode} {Status} {Message}", newAlert.Id, (int)response.StatusCode, response.StatusCode, errorString);
            }
        }

        private static DateTimeOffset GetSubmitTime(ContactAlert newAlert, TimeZoneInfo timeZoneInfo)
            => TimeZoneInfo.ConvertTime(newAlert.DateRequested ?? newAlert.DateCreated, timeZoneInfo);
    }
}