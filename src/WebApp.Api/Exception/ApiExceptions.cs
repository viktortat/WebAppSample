namespace WebApp.Api.Exception;

#pragma warning disable 1591
public class NotFoundException : System.Exception
{
    public NotFoundException(string message) : base(message)
    { }
}
public class BadRequestException : System.Exception
{
    public BadRequestException(string message) : base(message)
    { }
}
#pragma warning restore 1591
public class ApiException<T> : System.Exception
{
    public T Data { get; }

    public ApiException(string message, T data) : base(message)
    {
        Data = data;
    }

    public ApiException(string message, T data, System.Exception innerException) : base(message, innerException)
    {
        Data = data;
    }
}
public class NotFoundException<T> : System.Exception
{
    public T Data { get; }

    public NotFoundException(T data) : base($"The item '{data}' was not found.")
    {
        Data = data;
    }

    public NotFoundException(T data, string message) : base(message)
    {
        Data = data;
    }

    public NotFoundException(T data, string message, System.Exception innerException) : base(message, innerException)
    {
        Data = data;
    }

    public NotFoundException(string ffff)
    {
        throw new NotImplementedException();
    }
}

public class NotFoundException2<T> : System.Exception
{
    public T Item { get; }

    public NotFoundException2(T item)
        : base($"The item '{item}' was not found.")
    {
        Item = item;
    }

    public NotFoundException2(T item, string message)
        : base(message)
    {
        Item = item;
    }

    public NotFoundException2(T item, string message, System.Exception innerException)
        : base(message, innerException)
    {
        Item = item;
    }
}