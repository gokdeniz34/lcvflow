namespace LcvFlow.Domain;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public static Result<T> Success() => new()
    {
        IsSuccess = true
    };
    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };
    public static Result<T> Failure(string message) => new()
    {
        IsSuccess = false,
        Message = message
    };
}
