using TheKingInYellow.Core.Domain.Primitives;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class ResultTests
{
    [Fact]
    public void Result_Success_CreatesSuccessfulResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(string.Empty, result.Error);
    }

    [Fact]
    public void Result_Failure_CreatesFailedResultWithError()
    {
        var errorMessage = "Algo deu errado durante a execução.";
        var result = Result.Failure(errorMessage);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(errorMessage, result.Error);
    }

    [Fact]
    public void ResultT_Success_CreatesSuccessfulResultWithValue()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(42, result.Value);
        Assert.Equal(string.Empty, result.Error);
    }

    [Fact]
    public void ResultT_Failure_CreatesFailedResultWithFallbackValue()
    {
        var errorMessage = "Valor inválido.";
        var result = Result<string>.Failure(errorMessage, "fallback_valor");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("fallback_valor", result.Value);
        Assert.Equal(errorMessage, result.Error);
    }

    [Fact]
    public void ResultT_Failure_WithDefaultFallback_ReturnsDefault()
    {
        var errorMessage = "Erro sem fallback.";
        var result = Result<int>.Failure(errorMessage);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(0, result.Value);
        Assert.Equal(errorMessage, result.Error);
    }

    [Fact]
    public void ResultT_ImplicitConversion_ConvertsToSuccessResult()
    {
        Result<string> result = "valor_automatico";

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal("valor_automatico", result.Value);
        Assert.Equal(string.Empty, result.Error);
    }
}
