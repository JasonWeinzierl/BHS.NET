using BHS.Contracts;
using BHS.Domain.ContactUs;
using BHS.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace BHS.Domain.Tests.ContactUs;

public class ContactUsServiceTests
{
    private readonly ContactUsOptions _settings = new();
    private readonly IContactAlertRepository _repo = Substitute.For<IContactAlertRepository>();
    private readonly IEmailAdapter _emailAdapter = Substitute.For<IEmailAdapter>();
    private readonly ILogger<ContactUsService> _logger = Substitute.For<ILogger<ContactUsService>>();

    private ContactUsService Subject => new(Options.Create(_settings), _repo, _emailAdapter, _logger);

    public class AddRequest : ContactUsServiceTests
    {
        [Fact]
        public async Task OnBodyEmpty_InsertsAndSendsMessage()
        {
            // Arrange
            var request = new ContactAlertRequest(default, "x", default, default, null);
            _repo
                .Insert(request, default)
                .Returns(new ContactAlert("1", default, string.Empty, default, default, default));
            _emailAdapter
                .Send(Arg.Any<EmailMessageRequest>(), Arg.Any<CancellationToken>())
                .Returns(new EmailMessageResponse(System.Net.HttpStatusCode.OK, new StringContent("")));

            // Act
            var result = await Subject.AddRequest(request);

            // Assert
            Assert.NotNull(result);
            await _repo
                .Received(1)
                .Insert(request, default);
            await _emailAdapter
                .Received(1)
                .Send(Arg.Any<EmailMessageRequest>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task OnBodyHasValue_DoesNothing()
        {
            // Arrange
            var request = new ContactAlertRequest(default, string.Empty, default, default, "something");

            // Act
            var result = await Subject.AddRequest(request);

            // Assert
            Assert.Null(result);
            await _repo
                .DidNotReceiveWithAnyArgs()
                .Insert(Arg.Any<ContactAlertRequest>(), default);
            await _emailAdapter
                .DidNotReceiveWithAnyArgs()
                .Send(Arg.Any<EmailMessageRequest>(), default);
        }

        [Fact]
        public async Task OnEmailEmpty_Throws()
        {
            // Arrange
            var request = new ContactAlertRequest(default, string.Empty, default, default, null);

            await Assert.ThrowsAsync<InvalidContactRequestException>(async () =>
            {
                // Act
                _ = await Subject.AddRequest(request);
            });

            // Assert
            await _repo
                .DidNotReceiveWithAnyArgs()
                .Insert(Arg.Any<ContactAlertRequest>(), default);
            await _emailAdapter
                .DidNotReceiveWithAnyArgs()
                .Send(Arg.Any<EmailMessageRequest>());
        }
    }
}
