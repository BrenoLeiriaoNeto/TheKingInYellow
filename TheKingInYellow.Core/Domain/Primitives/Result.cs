using System.Diagnostics;

namespace TheKingInYellow.Core.Domain.Primitives;

/// <summary>
/// Representa o resultado de uma operação que não retorna valor, indicando sucesso ou falha com mensagem de erro.
/// </summary>
public readonly struct Result
{
    /// <summary>
    /// Indica se a operação foi concluída com sucesso.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica se a operação falhou.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Contém a mensagem de erro associada à falha da operação, ou uma cadeia vazia em caso de sucesso.
    /// </summary>
    public string Error { get; }

    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Cria uma instância de <see cref="Result"/> indicando sucesso na operação.
    /// </summary>
    /// <returns>Uma nova instância de <see cref="Result"/> com sucesso.</returns>
    public static Result Success() => new(true, string.Empty);

    /// <summary>
    /// Cria uma instância de <see cref="Result"/> indicando falha na operação com uma mensagem explicativa.
    /// </summary>
    /// <param name="error">A descrição do erro ocorrido.</param>
    /// <returns>Uma nova instância de <see cref="Result"/> com falha.</returns>
    public static Result Failure(string error)
    {
        Debug.WriteLine($"[Result Failure] {error}");
        return new Result(false, error);
    }
}

/// <summary>
/// Representa o resultado de uma operação que retorna um valor do tipo <typeparamref name="T"/>, indicando sucesso ou falha com mensagem de erro.
/// </summary>
/// <typeparam name="T">O tipo do valor retornado pela operação.</typeparam>
public readonly struct Result<T>
{
    /// <summary>
    /// O valor retornado pela operação quando bem-sucedida, ou o valor de fallback em caso de falha.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Indica se a operação foi concluída com sucesso.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica se a operação falhou.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Contém a mensagem de erro associada à falha da operação, ou uma cadeia vazia em caso de sucesso.
    /// </summary>
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Cria uma instância de <see cref="Result{T}"/> indicando sucesso na operação e contendo o valor produzido.
    /// </summary>
    /// <param name="value">O valor resultante da operação.</param>
    /// <returns>Uma nova instância de <see cref="Result{T}"/> com sucesso.</returns>
    public static Result<T> Success(T value) => new(value, true, string.Empty);

    /// <summary>
    /// Cria uma instância de <see cref="Result{T}"/> indicando falha na operação com uma mensagem explicativa e um valor de fallback opcional.
    /// </summary>
    /// <param name="error">A descrição do erro ocorrido.</param>
    /// <param name="fallbackValue">O valor padrão ou de contingência retornado em caso de falha.</param>
    /// <returns>Uma nova instância de <see cref="Result{T}"/> com falha.</returns>
    public static Result<T> Failure(string error, T? fallbackValue = default)
    {
        Debug.WriteLine($"[Result<T> Failure] {error}");
        return new Result<T>(fallbackValue, false, error);
    }

    /// <summary>
    /// Converte implicitamente um valor do tipo <typeparamref name="T"/> em um <see cref="Result{T}"/> bem-sucedido.
    /// </summary>
    /// <param name="value">O valor a ser encapsulado no resultado.</param>
    public static implicit operator Result<T>(T value) => Success(value);
}