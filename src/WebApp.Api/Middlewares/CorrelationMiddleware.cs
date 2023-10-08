namespace WebApp.Api.Middlewares;

internal class CorrelationMiddleware
{
    internal const string CorrelationHeaderKey = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = Guid.NewGuid();
        if (context.Request != null)
        {
            context.Request.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
        }
        await this._next.Invoke(context);
    }
}

public interface IExecutionContextAccessor
{
    Guid CorrelationId { get; }

    bool IsAvailable { get; }
}

public class ExecutionContextAccessor : IExecutionContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CorrelationId
    {
        get
        {
            if (IsAvailable && _httpContextAccessor.HttpContext!.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey))
            {
                return Guid.Parse(
                    _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]!);
            }
            throw new ApplicationException("Http context and correlation id is not available");
        }
    }

    public bool IsAvailable => _httpContextAccessor.HttpContext != null;
}