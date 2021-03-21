using BHS.Contracts;
using BHS.Model.DataAccess;
using BHS.Model.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BHS.BusinessLogic.Tests
{
    public class ContactUsServiceTests
    {
        private readonly ContactUsService _subject;

        private readonly Mock<IContactAlertRepository> _mockRepo;
        private readonly Mock<ISendGridClient> _sgClient;
        private readonly Mock<ILogger<ContactUsService>> _logger;

        public ContactUsServiceTests()
        {
            _mockRepo = new Mock<IContactAlertRepository>();
            _sgClient = new Mock<ISendGridClient>();
            _logger = new Mock<ILogger<ContactUsService>>();
            _subject = new ContactUsService(_mockRepo.Object, _sgClient.Object, _logger.Object);
        }

        [Fact]
        public async Task AddRequest_OnBodyEmpty_InsertsAndSendsMessage()
        {
            var request = new ContactAlertRequest(default, "x", default, default, null);
            _mockRepo.Setup(r => r.Insert(It.IsAny<ContactAlertRequest>()))
                .Returns(() => Task.FromResult(new ContactAlert(default, default, string.Empty, default, default, default)));
            _sgClient.Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(new Response(System.Net.HttpStatusCode.OK, default, default)));

            await _subject.AddRequest(request);

            _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>()), Times.Once, "Expected insert to be called.");
            _sgClient.Verify(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once, "Expected message to be sent.");
        }

        [Fact]
        public async Task AddRequest_OnBodyHasValue_DoesNothing()
        {
            var request = new ContactAlertRequest(default, string.Empty, default, default, "something");

            await _subject.AddRequest(request);

            _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>()), Times.Never, "Expected insert to never be called if body (honeypot) has value.");
            _sgClient.Verify(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never, "Expected message to never be sent if body (honeypot) has value.");
        }

        [Fact]
        public async Task AddRequest_OnEmailEmpty_Throws()
        {
            var request = new ContactAlertRequest(default, string.Empty, default, default, null);

            await Assert.ThrowsAsync<BadRequestException>(async () =>
            {
                await _subject.AddRequest(request);
            });

            _mockRepo.Verify(r => r.Insert(It.IsAny<ContactAlertRequest>()), Times.Never, "Expected insert to never be called if email is empty.");
            _sgClient.Verify(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never, "Expected message to never be sent if email is empty.");
        }
    }
}
