using System.Diagnostics;

namespace TheKingInYellow.Core.Domain.Primitives;

/// <summary>
/// Representa o resultado de uma operação que não retorna valor.
/// </summary>
public readonly struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string error)
    {
        Debug.Assert(false, $"[Result Failure] {error}");
        return new Result(false, error);
    }
}

/// <summary>
/// Representa o resultado de uma operação que retorna um valor do tipo T.
/// </summary>
/// <typeparam name="T">O valor retornado pela operação.</typeparam>
public readonly struct Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result<T> Success(T value) => new(value, true, string.Empty);

    public static Result<T> Failure(string error, T? fallbackValue = default)
    {
        Debug.Assert(false, $"[Result<T> Failure] {error}");
        return new Result<T>(fallbackValue, false, error);
    }

    public static implicit operator Result<T>(T value) => Success(value);
    
}