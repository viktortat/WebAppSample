using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Models;
using WebApp.Api.Services;

namespace WebApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzasController : ControllerBase
{
    private readonly IPizzaService _dbSrv;

    public PizzasController(IPizzaService dbSrv)
    {
        _dbSrv = dbSrv;
    }

    /// <summary>
    /// Returns the price of a frame based on its dimensions.
    /// </summary>
    /// <param name="Height">The height of the frame.</param>
    /// <param name="Width">The width of the frame.</param>
    /// <returns>The price, in dollars, of the picture frame.</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /api/priceframe/5/10
    ///
    /// </remarks>
    /// <response code="200">Returns the cost of the frame in dollars.</response>
    /// <response code="400">If the amount of frame material needed is less than 20 inches or greater than 1000 inches.</response>
    [HttpGet("{Height}/{Width}", Name = nameof(GetPrice))]
    [Produces("text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<string> GetPrice(string Height, string Width)
    {
        string result;
        result = CalculatePrice(Double.Parse(Height), Double.Parse(Width));
        if (result == "not valid")
        {
            return BadRequest(result);
        }
        else
        {
            return Ok($"The cost of a {Height}x{Width} frame is ${result}");
        }
    }

    private string CalculatePrice(double Height, double Width)
    {
        if (Height < 20) return "not valid";
        if (Width < 20) return "not valid";
        if (Height > 1000) return "not valid";
        if (Width > 1000) return "not valid";
        return $"{Height * Width}";
    }

    /// <summary>
    /// GET all Pizzas
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll() => await _dbSrv.GetAll();

    /// <summary>
    /// GET by Id action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _dbSrv.Get(id);
        if (result.StatusCode == StatusCodes.Status404NotFound) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// POST action 
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Product product)
    {
        var pathBase = HttpContext.Request.PathBase;

        await _dbSrv.Create(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    /// <summary>
    /// PUT action 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name)) return BadRequest(ApiResult<Product>.Failure("Name не может быть пустым", stCode: StatusCodes.Status400BadRequest));
        if (id == 0) return BadRequest(ApiResult<Product>.Failure("id must be not 0", stCode: StatusCodes.Status400BadRequest));

        if (product.Id == 0) product.Id = id;
        var result = await _dbSrv.Get(id);
        if (result.StatusCode == StatusCodes.Status404NotFound) return NotFound(result);
        await _dbSrv.Update(product);
        return NoContent();
    }

    /// <summary>
    /// DELETE action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _dbSrv.Get(id);
        if (result.StatusCode == StatusCodes.Status404NotFound) return NotFound(result);
        await _dbSrv.Delete(id);
        return NoContent();
    }
}