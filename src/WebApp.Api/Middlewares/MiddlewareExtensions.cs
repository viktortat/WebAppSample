namespace WebApp.Api.Middlewares;

public class MiddlewareSettings
{
    public bool UseTimeLoggingMiddleware { get; set; }
    public bool UseCultureMiddleware { get; set; }
    public bool UseIntentionalDelayMiddleware { get; set; }
    public bool UseErrorHandlingMiddleware { get; set; }
    public bool UseCorrelationMiddleware { get; set; }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationMiddleware>();
    }      
    
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }   

    /// <summary>
    /// Adds the Layout middleware, which does not change processing at all.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLayoutMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LayoutMiddleware>();
    }

    /// <summary>
    /// Adds the Logging middleware, which logs the incoming request's path.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggingMiddleware>();
    }

    /// <summary>
    /// Adds the Culture middleware, which sets the current culture based on the query string.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCultureMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CultureMiddleware>();
    }

    /// <summary>
    /// Adds the time logging middleware, which logs how long it takes for the system to return a response.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseTimeLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TimeLoggingMiddleware>();
    }

    /// <summary>
    /// Adds the intentional delay middleware, which makes the pipeline pause for 100ms both on request and again on response.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseIntentionalDelayMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IntentionalDelayMiddleware>();
    }
}