using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WebApp.Api.Controllers;
using WebApp.Api.Models;
using WebApp.Api.Services;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApp.Tests.Controllers
{
    //[Collection("Product")]
    [Trait("Product","Controller")]
    public class PizzasControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ITestOutputHelper _oConsole;
        private readonly HttpClient _client;
        private readonly Mock<IPizzaService> _pizzaServiceMock;

        public static List<Product> MyTestPizzas = new()
        {
            new() { Id = 1, Name = "Margherita", Description = "pizza 1", Price = 10.99 },
            new() { Id = 2, Name = "Pepperoni", Description = "pizza 2", Price = 12.99 }
        };

        public PizzasControllerIntegrationTests(WebApplicationFactory<Program> factory, ITestOutputHelper oConsole)
        {
            _factory = factory;
            _oConsole = oConsole;
            _client = factory.CreateClient();
            _pizzaServiceMock = new Mock<IPizzaService>();
            
            //var fixture = new Fixture();
            //var pizzas = fixture.CreateMany<Product>(10).ToArray();
        }
        
        [Fact]
        public async Task Get_t1_should_throw_an_exception()
        {
            // Arrange
            var _controller = new Mock<TestController>();
            var logger = new Mock<ILogger<TestController>>();
            
        //    // Act
        //    Func<Task<IActionResult>> act = async () => await _controller.Ge();

        //    // Assert
        //    await act.Should().ThrowAsync<Exception>().WithMessage("Exception while fetching all the students from the storage.");

        }

        [Fact]
        public async Task HttpStatusCodeHandler_should_return_a_404_response()
        {

            // Act
            var response = await _client.GetAsync("/Error/404");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("404, Requested resource was not found");
        }

        #region GetPrice

        [Fact]
        public async Task GetPrice_shouldReturnBadRequestWhenWidthIsLessThan20()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "/pizzas/30/10";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);
        }



        [Fact]
        public async Task GetPrice_shouldReturnBadRequestWhenHeightIsGreaterThan1000()
        {
            // Arrange
            var url = "/pizzas/5000/30";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GetPrice_shouldReturnBadRequestWhenWidthIsGreaterThan1000()
        {
            // Arrange
            var url = "/pizzas/30/5000";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GetPrice_shouldReturnBadRequestWhenHeightIsLessThan20()
        {
            // Arrange
            var url = "/pizzas/10/30";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            // Arrange
            var url = $"/pizzas/2";
            // Act
            var response = await _client.GetAsync(url);
            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetPrice_shouldReturnOkWhenParametersAreValid()
        {
            // Arrange
            var url = "/pizzas/30/40";

            // Act
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status200OK);
            content.Should().Be("The cost of a 30x40 frame is $1200");
        }

        #endregion

        #region GetAll

        [Fact]
        [Trait("GetAll","Return all Pizzas")]
        public async Task GetAll_shouldReturnAllPizzas()
        {
            // Arrange
            _pizzaServiceMock.Setup(m => m.GetAll()).ReturnsAsync(MyTestPizzas);
            //var cnt = new PizzasController(_pizzaServiceMock.Object);
            //var okResult = await cnt.GetAll();
            var expectedJson = JsonSerializer.Serialize(MyTestPizzas);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(_ => _pizzaServiceMock.Object); });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/pizzas");
            var responseContent = await response.Content.ReadAsStringAsync();
            _oConsole.WriteLine(responseContent);
            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        [Trait("GetAll","Return List Of Pizzas2")]
        public async Task GetAll_shouldReturnListOfPizzas2()
        {
            // Arrange
            var pizza1 = new Product { Id = 1, Name = "Pepperoni", Price = 25 };
            var pizza2 = new Product { Id = 2, Name = "Mushroom", Price = 170, Description = "Test" };

            _pizzaServiceMock.Setup(srv => srv.GetAll()).ReturnsAsync(new List<Product> { pizza1, pizza2 });
            var controller = new PizzasController(_pizzaServiceMock.Object);

            // Act
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(_ => _pizzaServiceMock.Object); });
            }).CreateClient();

            var response = await client.GetAsync("/pizzas");
            var pizzas = await response.Content.ReadFromJsonAsync<List<Product>>();
            //var pizzas = await response.Content.ReadAsAsync<List<Product>>();

            //// Assert
            response.EnsureSuccessStatusCode();
            pizzas.Should().NotBeNullOrEmpty();
            pizzas.Should().ContainEquivalentOf(pizza1);
            pizzas.Should().ContainEquivalentOf(pizza2);
        }

        [Fact]
        public async Task GetAll_shouldReturnListOfPizzas()
        {
            // Arrange
            _pizzaServiceMock.Setup(srv => srv.GetAll())
                .ReturnsAsync(new List<Product>
                    { new Product { Id = 1, Name = "Margarita" }, new Product { Id = 2, Name = "Pepperoni" } });
            var client = GetMyClient();

            // Act
            var response = await client.GetAsync("/pizzas");
            var content = await response.Content.ReadAsStringAsync();
            var pizzas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(content);

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status200OK);
            pizzas.Count.Should().Be(2);
        }

        #endregion

        #region Get_ID

        [Fact]
        public async Task Get_shouldReturnNotFoundForInvalidId()
        {
            // Arrange
            var notFoundResult = ApiResult<Product>.Failure("Not Found", StatusCodes.Status404NotFound);
            _pizzaServiceMock.Setup(repo => repo.Get(1))
                .ReturnsAsync(notFoundResult);
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(_ => _pizzaServiceMock.Object); });
            }).CreateClient();

            // Act
            var response = await client.GetAsync("/pizzas/1");

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Get_shouldReturnCorrectPizza(int id)
        {
            // Arrange
            var pizza = new Product { Id = id, Name = "Margherita", Price = 8.99 };
            _pizzaServiceMock.Setup(x => x.Get(id)).ReturnsAsync(ApiResult<Product>.Success(pizza));
            var url = $"/pizzas/{id}";

            // Act
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(_ => _pizzaServiceMock.Object); });
            }).CreateClient();
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResult<Product>>(responseString);
            result.Should().BeEquivalentTo(ApiResult<Product>.Success(pizza));
        }

        [Fact]
        public async Task Get_shouldReturnPizzaById()
        {
            // Arrange
            var pizza1 = new Product { Id = 1, Name = "Pepperoni", Price = 152 };
            _pizzaServiceMock.Setup(srv => srv.Get(1)).ReturnsAsync(ApiResult<Product>.Success(pizza1));
            //var controller = new PizzasController(_pizzaServiceMock.Object);

            // Act
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(_ => _pizzaServiceMock.Object); });
            }).CreateClient();
            var response = await client.GetAsync("/pizzas/1");
            var responseContent = await response.Content.ReadAsStringAsync();

            var pizza = JsonConvert.DeserializeObject<ApiResult<Product>>(responseContent);
            //var pizza = await response.Content.ReadFromJsonAsync<ApiResult<Product>>();

            // Assert
            response.EnsureSuccessStatusCode();
            pizza.Data.Should().BeEquivalentTo(pizza1);
        }

        [Fact]
        public async Task Get_shouldReturnNotFoundWhenPizzaDoesNotExist()
        {
            // Arrange
            _pizzaServiceMock.Setup(srv => srv.Get(1))
                .ReturnsAsync(ApiResult<Product>.Failure("Product not found", StatusCodes.Status404NotFound));
            //var controller = new PizzasController(_pizzaServiceMock.Object);
            var client = GetMyClient();
            // Act
            var response = await client.GetAsync("/pizzas/1");
            var result = await response.Content.ReadAsStringAsync();
            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status404NotFound);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenPizzaIsCreated()
        {
            // Arrange
            var newPizza = new Product { Name = "Hawaiian", Price = 10.99 };
            var createdPizza = new Product { Id = 1, Name = newPizza.Name, Price = newPizza.Price };
            newPizza.Id = 1;
            _pizzaServiceMock.Setup(x => x.Create(newPizza)).ReturnsAsync(ApiResult<Product>.Success(newPizza));

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddSingleton(_pizzaServiceMock.Object); });
            }).CreateClient();
            var requestBody =
                new StringContent(JsonConvert.SerializeObject(newPizza), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/pizzas", requestBody);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responsePizza = JsonConvert.DeserializeObject<Product>(responseContent);

            //// Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status201Created);
            //response.Headers.Location.Should().Be($"/pizzas/{createdPizza.Id}");
            responsePizza.Should().BeEquivalentTo(createdPizza);
        }

        [Fact]
        public async Task Create_shouldAddNewPizza()
        {
            // Arrange
            var newPizza = new Product { Name = "Hawaiian" };
            _pizzaServiceMock.Setup(srv => srv.Create(newPizza)).ReturnsAsync(ApiResult<Product>.Success(newPizza));
            var controller = new PizzasController(_pizzaServiceMock.Object);
            var client = GetMyClient();
            // Act
            var response = await client.PostAsJsonAsync("/pizzas", newPizza);

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status201Created);
            //response.Headers.Location.Should().Be($"/pizzas/{newPizza.Id}");
        }

        #endregion
        
        #region Update

        [Theory]
        [InlineData(0)]
        public async Task Update_ReturnsBadRequest_WhenIdIsZero(int id)
        {
            // Arrange
            var content = new StringContent(JsonConvert.SerializeObject(new Product()), Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PutAsync($"/pizzas/{id}", content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult<Product>>(result);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResult.IsSuccess.Should().BeFalse();
            //apiResult.Message.Should().Be("id must be not 0");
        }

        [Theory]
        [InlineData(1)]
        public async Task Update_ReturnsNotFound_WhenPizzaNotFound(int id)
        {
            // Arrange
            _pizzaServiceMock.Setup(srv => srv.Get(id))
                .ReturnsAsync(ApiResult<Product>.Failure("Product not found", stCode: StatusCodes.Status404NotFound));
            var client = GetMyClient();
            var content = new StringContent(JsonConvert.SerializeObject(new Product { Id = id }), Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PutAsync($"/pizzas/{id}", content);
            var result = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult<Product>>(result);

            // Assert
            //response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            //apiResult.IsSuccess.Should().BeFalse();
            //apiResult.Message.Should().Be("Product not found");
        }

        [Theory]
        [InlineData(1)]
        public async Task Update_ReturnsNoContent_WhenPizzaUpdated(int id)
        {
            // Arrange

            _pizzaServiceMock.Setup(srv => srv.Get(id)).ReturnsAsync(ApiResult<Product>.Success(new Product { Id = id }));
            var client = GetMyClient();
            var content = new StringContent(JsonConvert.SerializeObject(new Product { Id = id, Name = "Updated Product" }),
                Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/pizzas/{id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _pizzaServiceMock.Verify(srv => srv.Update(It.Is<Product>(p => p.Id == id && p.Name == "Updated Product")),
                Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_shouldDeletePizza_WhenGivenValidId()
        {
            // Arrange
            var id = 1;
            _pizzaServiceMock.Setup(service => service.Get(id))
                .ReturnsAsync(ApiResult<Product>.Success(new Product { Id = id }));
            var client = GetMyClient();

            // Act
            var response = await client.DeleteAsync($"/pizzas/{id}");

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status204NoContent);
            _pizzaServiceMock.Verify(service => service.Delete(id), Times.Once);
        }

        [Fact]
        public async Task Delete_shouldReturnNotFound_WhenGivenInvalidId()
        {
            // Arrange
            var id = 0;

            _pizzaServiceMock.Setup(service => service.Get(id))
                .ReturnsAsync(ApiResult<Product>.Failure("Product not found", StatusCodes.Status404NotFound));
            var client = GetMyClient();
            // Act
            var response = await client.DeleteAsync($"/pizzas/{id}");
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResult<Product>>(responseString);
            //var result = await response.Content.ReadFromJsonAsync<ApiResult<Product>>();

            // Assert
            ((int)response.StatusCode).Should().Be(StatusCodes.Status404NotFound);
            result.ErrorMessage.Should().Be("Product not found");
            _pizzaServiceMock.Verify(service => service.Delete(id), Times.Never);
        }

        #endregion

        private HttpClient GetMyClient()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped(_ => _pizzaServiceMock.Object); });
            }).CreateClient();
            return client;
        }

        #region SkipTests
        /*
        [Fact(Skip = "test")]
        public async Task TestDbSet()
        {
            IQueryable<Product> pizzas = new List<Product>
            {
                new Product { Id = 1, Name = "Margherita", Description = "pizza 1", Price = 10.99 },
                new Product { Id = 2, Name = "Pepperoni", Description = "pizza 2", Price = 12.99 }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DbTest")
                .Options;
            var mockContext = new Mock<ApplicationDbContext>(options);

            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(pizzas.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(pizzas.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(pizzas.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(pizzas.GetEnumerator());
            
            mockContext.Setup(c => c.Pizzas).Returns(mockSet.Object);
        }
        */
        #endregion

    }


    
}
