using TheKingInYellow.Core.Domain.ValueObjects;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class StatValueTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(10, 5, 2)]
    [InlineData(50, 25, 10)]
    [InlineData(51, 25, 10)]
    [InlineData(75, 37, 15)]
    [InlineData(90, 45, 18)]
    [InlineData(99, 49, 19)]
    public void StatValue_CalculatesHalfAndFifthCorrectly(int baseVal, int expectedHalf,
        int expectedFifth)
    {
        var stat = new StatValue(baseVal);

        Assert.Equal(baseVal, stat.Base);
        Assert.Equal(expectedHalf, stat.Half);
        Assert.Equal(expectedFifth, stat.Fifth);
    }

    [Theory]
    [InlineData(-10, 0)]
    [InlineData(-1, 0)]
    [InlineData(100, 99)]
    [InlineData(150, 99)]
    public void StatValue_ClampsValueBetweenZeroAndNinetyNine(int inputVal, int expectedBase)
    {
        var stat = new StatValue(inputVal);

        Assert.Equal(expectedBase, stat.Base);
    }

    [Fact]
    public void StatValue_ImplicitConversionToInt_ReturnsBaseValue()
    {
        var stat = new StatValue(70);
        int intVal = stat;

        Assert.Equal(70, intVal);
    }

    [Fact]
    public void StatValue_ImplicitConversionFromInt_CreatesStatValueWithClampedBase()
    {
        StatValue stat = 85;

        Assert.Equal(85, stat.Base);
    }

    [Fact]
    public void StatValue_ToString_ReturnsBaseStringRepresentation()
    {
        var stat = new StatValue(65);

        Assert.Equal("65", stat.ToString());
    }
}