using Project.Generic;
using System.Net;

namespace Project.Tests.Unit;
internal class MockEmailClient : IEmailClient
{
    public Task<HttpResponseMessage> SendEmail(EmailModel model)
    {
        LatestEmail = model;
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }

    public EmailModel? LatestEmail { get; private set; }

}

[TestFixture]
public class EmailTests
{
    private EmailService _emailService = null!;
    private MockEmailClient _client = null!;

    [SetUp]
    public void Setup()
    {
        _client = new MockEmailClient();
        _emailService = new EmailService(_client);
    }


    [Test]
    public async Task SendEmailShouldCompleteSuccesfullyWithCorrectParameters()
    {
        // Arrange
        string recipient = "test@test.com";
        string subject = "subject";
        string body = "body";

        // Act

        await _emailService.SendEmail(recipient, subject, body);

        // Assert
        Assert.That(_client.LatestEmail?.Recipients?.FirstOrDefault()?.Email, Is.Not.Null.And.EqualTo(recipient));
    }

    [Test]
    public void SendEmailShouldErrorWhenRecipientInvalidEmail()
    {
        // Arrange
        string recipient = "not an email";
        string subject = "subject";
        string body = "body";


        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _emailService.SendEmail(recipient, subject, body);
        });
    }

    [Test]
    public void SendEmailShouldErrorWhenRecipientSubjectMissing()
    {
        // Arrange
        string recipient = "test@test.com";
        string subject = "";
        string body = "body";


        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _emailService.SendEmail(recipient, subject, body);
        });
    }
}