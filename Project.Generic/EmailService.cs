using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Generic;

public interface IEmailService
{
    Task SendEmail(string recipient, string subject, string body);
}



public class EmailService(IEmailClient emailClient) : IEmailService
{
    private readonly IEmailClient _emailClient = emailClient;

    public async Task SendEmail(string recipient, string subject, string body)
    {
        if (string.IsNullOrEmpty(recipient))
            throw new ArgumentException($"'{nameof(recipient)}' cannot be null or empty.", nameof(recipient));
        if (string.IsNullOrEmpty(subject))
            throw new ArgumentException($"'{nameof(subject)}' cannot be null or empty.", nameof(subject));
        if (string.IsNullOrEmpty(body))
            throw new ArgumentException($"'{nameof(body)}' cannot be null or empty.", nameof(body));

        if (!new EmailAddressAttribute().IsValid(recipient))
            throw new ArgumentException($"'{nameof(recipient)}' is not a valid email address.", nameof(recipient));

        body = body.Replace('"', '\'');

        var model = new EmailModel
        {
            FromEmail = "noreply@nathansykesproject.co.uk",
            FromName = "Project Nathan NoReply",
            Subject = subject,
            HtmlPart = body,
            Recipients =
            [
                new Recipient
                {
                    Email = recipient
                }
            ]
        };

        var response = await _emailClient.SendEmail(model);

        response.EnsureSuccessStatusCode();
    }
}

public interface IEmailClient
{
    public Task<HttpResponseMessage> SendEmail(EmailModel model);
}

public class MailJetEmailClient(IConfiguration configuration) : IEmailClient
{
    private static HttpClient _client = new();
    private readonly IConfiguration _configuration = configuration;

    public async Task<HttpResponseMessage> SendEmail(EmailModel model)
    {
        var apiKey = _configuration["Mailjet:ApiKey"];
        var apiSecret = _configuration["Mailjet:ApiSecret"];

        var content = JsonConvert.SerializeObject(model);
        content = content.Replace("HtmlPart", "Html-part");
        content = content.Replace("TextPart", "Text-part");
        var message = new HttpRequestMessage(HttpMethod.Post, "https://api.mailjet.com/v3/send")
        {
            Content = new StringContent(content, Encoding.UTF8, "application/json"),
        };
        message.SetBasicAuthentication(apiKey, apiSecret);

        _client ??= new HttpClient();

        var response = await _client.SendAsync(message);
        return response;
    }
}


public class EmailModel
{
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }
    public string? Subject { get; set; }
    public string? HtmlPart { get; set; }
    public string? TextPart { get; set; }
    public Recipient[]? Recipients { get; set; }
}
public class Recipient
{
    public string? Email { get; set; }
}