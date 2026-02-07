namespace LcvFlow.Domain.Common;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public string? Message { get; protected set; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string message) => new(false, message);
}

public class Result<T> : Result
{
    public T? Data { get; private set; }

    protected Result(T? data, bool isSuccess, string? message) 
        : base(isSuccess, message)
    {
        Data = data;
    }

    public static Result<T> Success(T data) => new(data, true, null);
    public new static Result<T> Failure(string message) => new(default, false, message);
}