using BHS.Contracts;
using BHS.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BHS.Domain.ContactUs;

public class ContactUsService : IContactUsService
{
    private readonly ContactUsOptions _options;
    private readonly IContactAlertRepository _contactAlertRepository;
    private readonly IEmailAdapter _emailAdapter;
    private readonly ILogger _logger;

    public ContactUsService(
        IOptions<ContactUsOptions> options,
        IContactAlertRepository contactAlertRepository,
        IEmailAdapter emailAdapter,
        ILogger<ContactUsService> logger)
    {
        _options = options.Value;
        _contactAlertRepository = contactAlertRepository;
        _emailAdapter = emailAdapter;
        _logger = logger;
    }

    public async Task<ContactAlert?> AddRequest(ContactAlertRequest request, CancellationToken cancellationToken = default)
    {
        if (FailsHoneypotCheck(request))
        {
            _logger.LogInformation("ContactAlertRequest failed honeypot check.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(request.EmailAddress))
            throw new InvalidContactRequestException("Email address is required.");

        var newAlert = await _contactAlertRepository.Insert(request, cancellationToken);
        await SendEmail(newAlert, cancellationToken);

        return newAlert;
    }

    private static bool FailsHoneypotCheck(ContactAlertRequest request) => !string.IsNullOrEmpty(request.Body);

    private async Task SendEmail(ContactAlert newAlert, CancellationToken cancellationToken)
    {
        // Windows Ids are supported on Linux as of .NET 6 #49412.
        var cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        var submitTime = GetSubmitTime(newAlert, cst);

        string userMessage = newAlert.Message ?? "(no message was provided)";

        var msg = new EmailMessageRequest(
            _options.ToAddresses,
            "Website Comment: " + newAlert.Name,
            @$"
<p>The following message has been submitted to the Belton Historical Society:</p>

<p>
Name: {newAlert.Name}<br>
Email Address: {newAlert.EmailAddress}<br>
Message:<br>
{userMessage}
</p>

<p><i>Submitted on {submitTime:dddd, d MMMM yyyy HH:mm:ss zzz} .</i></p>",
            userMessage);

        var response = await _emailAdapter.Send(msg, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("CreatedContactAlert #{Id}: {StatusCode} {Status}", newAlert.Id, (int)response.StatusCode, response.StatusCode);
        }
        else
        {
            string errorString = await response.Body.ReadAsStringAsync(cancellationToken);
            _logger.LogError("FailedContactAlert #{Id}: {StatusCode} {Status} {Message}", newAlert.Id, (int)response.StatusCode, response.StatusCode, errorString);
        }
    }

    private static DateTimeOffset GetSubmitTime(ContactAlert newAlert, TimeZoneInfo timeZoneInfo)
        => TimeZoneInfo.ConvertTime(newAlert.DateRequested ?? newAlert.DateCreated, timeZoneInfo);
}
