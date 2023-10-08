namespace WebApp.Api.Services;

public interface ILoggingService
{
    public void Log(LogLevel level, string message);
}

public class LoggingService : ILoggingService
{

    public void Log(LogLevel logLevel, string message)
    {
        //Implementation for logging
    }
}