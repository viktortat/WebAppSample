using System.Net;
using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WebApp.Api.Controllers;
using WebApp.Api.Models;
using WebApp.Api.Services;
using Xunit.Abstractions;

namespace WebApp.Tests.Controllers;

public class TestControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _oConsole;
    private readonly Mock<IPizzaService> _mockPizzaService;
    private readonly Mock<ILogger<TestController>> _mockLogger;
    private readonly TestController _controller;
    private readonly HttpClient _client;

    public TestControllerTests(WebApplicationFactory<Program> factory, ITestOutputHelper oConsole)
    {
        _factory = factory;
        _client = factory.CreateClient();

        _oConsole = oConsole;
        _mockPizzaService = new Mock<IPizzaService>();
        _mockLogger = new Mock<ILogger<TestController>>();
        _controller = new TestController(_mockPizzaService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Get_err_shouldThrowException()
    {
        // Arrange

        // Act
        Func<Task> act = async () => await _controller.Get_err();

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Exception while fetching all the students from the storage.");
    }
    
    [Fact]
    public async Task Get_should_return_a_version_response()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/version");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
            
        //var result = await response.Content.ReadFromJsonAsync<dynamic>();
        var result = JsonConvert.DeserializeObject<ApiVersion>(content) ;

        var expectedName = Assembly.GetExecutingAssembly().GetName().Name;
        result.name.Should().StartWith(expectedName!.Split(".").FirstOrDefault());

        var expectedAppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        result.ver_app.Should().Be(expectedAppVersion);

        var expectedMachineName = Environment.MachineName;
        //result.machineName.Should().Be(expectedMachineName);

        var expectedEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "none";
        result.env.Should().Be(expectedEnv);
    }

}