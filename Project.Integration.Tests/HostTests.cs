using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace Project.Integration.Tests;

[TestFixture]
public class HostTests
{
    private TestServer _testServer = null!;
    private HttpClient _testClient = null!;

    [SetUp]
    public void Setup()
    {
        var app = new WebApplicationFactory<Program>()
           .WithWebHostBuilder(builder =>
           {
               builder.ConfigureServices(services =>
               {
                   // set up servises
               });
           });
        _testServer = app.Server;
        _testClient = app.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _testServer.Dispose();
        _testClient.Dispose();
    }

    [Test]
    public void ApplicationShouldBuild()
    {
        // Act

        // Assert
        Assert.Pass();
    }

    [Test]
    public async Task ApplicationShouldRespond()
    {

        // Act
        var response = await _testClient.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(response.IsSuccessStatusCode);
        Assert.That(content, Is.EqualTo("Hello World"));
    }
}