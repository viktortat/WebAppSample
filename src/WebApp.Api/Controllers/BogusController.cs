using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Api.Data;
using WebApp.Api.Data.Fakers;
using WebApp.Api.Models;

namespace WebApp.Api.Controllers;

[ApiController]
//[Route("[controller]")]
public class BogusController : ControllerBase
{
    private readonly ILogger<BogusController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly AppSettings _appSettings;
    public BogusController(
        ILogger<BogusController> logger, 
        IConfiguration configuration,
        ApplicationDbContext dbContext
        )
    {
        _logger = logger;
        _dbContext = dbContext;
        _appSettings = configuration.Get<AppSettings>();
    }

    [HttpGet("api/Bogus/Customers")]
    public async Task<ActionResult<IEnumerable<Customer2>>> Bogus_Customers()
    {
        var customers = await _dbContext.Customers.ToListAsync();
        return Ok(customers);
    }
    
    [HttpGet("api/Bogus/Addresses")]
    public async Task<ActionResult<IEnumerable<Customer2>>> Bogus_Addresses()
    {
        var addresses = await _dbContext.Addresses.ToListAsync();
        return Ok(addresses);
    }

    [HttpGet("api/Bogus/100Customers")]
    public ActionResult<IEnumerable<Customer2>> Bogus_10Customers()
    {
        var customers = (new Fakers()).GetCustomerGenerator2().Generate(100);
        return customers;
    }

    [HttpGet("api/Bogus/100Persons")]
    public ActionResult<IEnumerable<Person>> Bogus_100Persons()
    {
        var f = new Fakers();
        var generatorPerson = f.getGeneratorPerson();
        var persons = generatorPerson.Generate(100);
        return persons;
    }
}