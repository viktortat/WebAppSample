using System.Text.Json.Serialization;

namespace WebApp.Api.Models;

internal class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}

public class ApiVersion
{
    public string? name { get; set; }
    public string? ver_app { get; set; }
    public string? machineName { get; set; }
    public string? correlation_id { get; set; }
    public string? ip_address { get; set; }
    public string? env { get; set; }
    public string? date { get; set; }
}

public class ApiResult<T>
{
    public bool IsSuccess { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Data { get; set; }
    public int StatusCode { get; set; } = StatusCodes.Status200OK;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; }

    public ApiResult(bool isSuccess = true, T data = default(T), string? errorMessage = null, int stCode = StatusCodes.Status200OK)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        StatusCode = stCode;
    }

    public static ApiResult<T> Failure(string errorMessage, int stCode = StatusCodes.Status500InternalServerError)
    {
        return new ApiResult<T>(false, default!, errorMessage, stCode);
    }

    public static ApiResult<T> Success(T data)
    {
        return new ApiResult<T>(true, data: data);
    }
}

public sealed class SaveChangesResult
{
    public SaveChangesResult() => this.Messages = new List<string>();

    public SaveChangesResult(string message) : this()
    {
        this.AddMessage(message);
    }

    public System.Exception? Exception { get; set; }

    public bool IsOk => this.Exception == null;

    public void AddMessage(string message) => this.Messages.Add(message);

    private List<string> Messages { get; }
}
