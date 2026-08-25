namespace Fluxo.BuildingBlocks.Application;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    protected Result(bool isSuccess, string? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("A successful result cannot carry an error.");
        if (!isSuccess && error is null)
            throw new InvalidOperationException("A failed result must carry an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    private Result(bool isSuccess, TValue? value, string? error) : base(isSuccess, error) => _value = value;

    public static Result<TValue> Success(TValue value) => new(true, value, null);
    public static new Result<TValue> Failure(string error) => new(false, default, error);
}
