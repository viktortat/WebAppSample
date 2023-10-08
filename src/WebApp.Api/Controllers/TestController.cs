using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using WebApp.Api.Services;
using ApiVersion = WebApp.Api.Models.ApiVersion;

namespace WebApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IPizzaService _dbSrv;
    private readonly ILogger<TestController> _logger;

    public TestController(IPizzaService dbSrv, ILogger<TestController> logger)
    {
        _dbSrv = dbSrv;
        _logger = logger;
    }

    [HttpGet("/slowtest4")]
    public async Task<IActionResult> Get_slowtest4(CancellationToken cancellationToken)
    {
        _logger.LogInformation("===================== Starting to do slow work");

        try
        {
            // slow async action, e.g. call external api
            await Task.Delay(10_000, cancellationToken);
            var message = "===================== Finished slow delay of 10 seconds.";
            _logger.LogInformation(message);
            return Ok(message);
        }
        catch (System.Exception)
        {
            _logger.LogError("===================== Cancel!!!");
            return BadRequest("ddd");
        }
    }



    [HttpGet("/slowtest")]
    public async Task<string> Get_slowtest(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to do slow work");

        // slow async action, e.g. call external api
        await Task.Delay(10_000, cancellationToken);

        var message = "Finished slow delay of 10 seconds.";

        _logger.LogInformation(message);

        return message;
    }

    [HttpGet("/slowtest2")]
    public async Task<string> Get_slowtest2(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to do slow work");

        for (var i = 0; i < 10; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // slow non-cancellable work
            Thread.Sleep(1000);
        }
        var message = "Finished slow delay of 10 seconds.";

        _logger.LogInformation(message);

        return message;
    }

    // https://localhost:5101/longrunningtask
    [HttpGet("/longrunningtask")]
    public async Task<IActionResult> Get_Longrunningtask(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Run(async () =>
            {
                for (var i = 0; i <= 100; i++)
                {
                    var dd = HttpContext.RequestAborted;

                    if (cancellationToken.IsCancellationRequested) break;
                    await Task.Delay(500, cancellationToken); 
                    Console.WriteLine($"Task completed {i}%");
                }
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($" ##### Task Cancel!!!");
            return BadRequest();
        }
        return Ok("Task Completed Successfully");
    }

    [HttpGet("/err")]
    public async Task<IActionResult> Get_err()
    {
        throw new System.Exception("Exception while fetching all the students from the storage.");
        _logger.LogInformation("Fetching all the Students from the storage");
        //var students = DataManager.GetAllStudents(); //simulation for the data base access
        throw new AccessViolationException("Violation Exception while accessing the resource.");
        //_logger.LogInfo($"Returning {students.Count} students.");
        return Ok();
    }

    [HttpGet("/Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                _logger.LogWarning("404, Requested resource was not found");
                return StatusCode((int)StatusCodes.Status404NotFound, new { ErrorMessage = "404, Requested resource was not found" });
                break;
        }
        return Ok("PageNotFound");
    }

    //https://localhost:5101/version
    //[HttpGet("/version")]
    //public async Task<IActionResult> Get()
    //{
    //    var correlationId = HttpContext.Request.Headers["X-Correlation-Id"].ToString();

    //    var _name = Assembly.GetExecutingAssembly().GetName().Name;
    //    var _ver_app = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
    //    var _machineName = Environment.MachineName;
    //    var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "none";
    //    var ipAddr = GetIpAddress();
    //    var result = new ApiVersion
    //    {
    //        name = _name,
    //        ver_app = _ver_app,
    //        machineName = _machineName,
    //        ip_address = ipAddr,
    //        env = _env,
    //        date = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
    //        //xuid = DateTime.Now.Ticks
    //        correlation_id = correlationId?[24..^0],
    //    };
    //    return Ok(result);
    //}

    [HttpGet("/version")]
    public async Task<IActionResult> Get()
    {
        var correlationId = HttpContext.Request.Headers["X-Correlation-Id"].ToString();

        var _name = Assembly.GetExecutingAssembly().GetName().Name;
        var _ver_app = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        var _machineName = Environment.MachineName;
        var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "none";
        var ipAddr = GetIpAddress();
        var result = new ApiVersion
        {
            name = _name,
            ver_app = _ver_app,
            machineName = _machineName,
            ip_address = ipAddr,
            env = _env,
            date = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"),
            //xuid = DateTime.Now.Ticks
            correlation_id = correlationId?[24..^0],
        };
        return Ok(result);
    }
    public static string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return string.Empty;
    }

}
