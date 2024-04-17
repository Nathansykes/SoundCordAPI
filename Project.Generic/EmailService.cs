using IdentityModel.Client;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace Project.Generic;
public class EmailService(IConfiguration configuration)
{

    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmail(string recipient, string subject, string body)
    {
        var apiKey = _configuration["Mailjet:ApiKey"];
        var apiSecret = _configuration["Mailjet:ApiSecret"];

        body = body.Replace('"', '\'');

        var model = new
        {
            FromEmail = "noreply@nathansykesproject.co.uk",
            FromName = "Project Nathan NoReply",
            Subject = subject,
            HtmlPart = body,
            Recipients = new[]
            {
                new
                {
                    Email = recipient
                }
            }
        };
        
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
        response.EnsureSuccessStatusCode();

    }
    private static HttpClient _client = new();
}