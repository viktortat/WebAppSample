using System.Net;
using System.Text.Json;
using WebApp.Api.Models;

namespace WebApp.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private static ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            //if(correctedPath != null)
            //{
            //    httpContext.Response. Redirect(httpContext.Request.PathBase + correctedPath +
            //                                   httpContext.Request.QueryString, permanent: true);
            //    return;
            //}
            await _next(httpContext);
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var message = exception switch
        {
            AccessViolationException => "Access violation error from the custom middleware",
            _ => "Internal Server Error! [44] "  
        };

#if DEBUG
        message = message +$"{exception.Message} # {exception?.InnerException}" ;
#endif

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(ApiResult<string>.Failure(message,context.Response.StatusCode)));
    }
}



/*
public class ErrorHandlingMiddleware2 //: IMiddleware
{
    private static ILogger _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware2(RequestDelegate next, ILogger<ErrorHandlingMiddleware2> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context, IWebHostEnvironment env)
    {
        try
        {
            var cr = context.Request;
            //_elRow.requestPath = $"{cr.Scheme} # {cr.Method} # {cr.Host} # {cr.Path} # {cr.QueryString}";

            //REQUEST
            var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            string requestBody = await new StreamReader(requestBodyStream).ReadToEndAsync();
            //_elRow.requestStr = String.Join(" # ", requestBody.GetLines(true));

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IWebHostEnvironment env)
    {
        HttpStatusCode status;
        string message;
        var stackTrace = String.Empty;

        var exceptionType = ex.GetType();
        if (exceptionType == typeof(BadRequestException))
        {
            message = ex.Message;
            status = HttpStatusCode.BadRequest;
        }
        else if (exceptionType == typeof(NotFoundException))
        {
            message = ex.Message;
            status = HttpStatusCode.NotFound;
        }
        else
        {
            status = HttpStatusCode.InternalServerError;
            message = ex.Message;
            if (env.IsEnvironment("Development")) stackTrace = ex.StackTrace;
        }

        var result = "";
        {
            //result = JsonConvert.SerializeObject(new { error = ex.Message });
            result = JsonSerializer.Serialize(new { error = ex.Message });
            context.Response.ContentType = "application/json";


            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //Обработка ошибки, к примеру, просто можем логировать сообщение ошибки
            _logger.LogError(0, ex.Message, "Ошибка обработки исключения");

            return context.Response.WriteAsync(result);
        }
    }


}

public class ErrorHandlingMiddleware2
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("Handled a global exception: " + ex.Message, ex);

            if (ex.Message.StartsWith("404 error") ||
                ex is NotFoundException)
            {
                httpContext.Response.Redirect("/error/404");
            }
            else
            {
                httpContext.Response.Redirect("/error/500");
            }
        }
    }
}
*/
