using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class InvestigatorCalculatorTests
{
    [Theory]
    [InlineData(50, 50, 10)]
    [InlineData(60, 70, 13)]
    [InlineData(15, 15, 3)]
    [InlineData(90, 90, 18)]
    public void CalculateMaxHp_ShouldReturnCorrectValue(int con, int siz, int expectedHp)
    {
        var result = InvestigatorCalculator.CalculateMaxHp(con, siz);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedHp, result.Value);
    }

    [Theory]
    [InlineData(-10, 50)]
    [InlineData(50, -5)]
    public void CalculateMaxHp_NegativeInputs_ReturnsFailure(int con, int siz)
    {
        var result = InvestigatorCalculator.CalculateMaxHp(con, siz);
        Assert.True(result.IsFailure);
        Assert.Equal("Os atributos CON e SIZ não podem ser negativos.", result.Error);
    }

    [Theory]
    [InlineData(50, 50)]
    [InlineData(85, 85)]
    [InlineData(0, 0)]
    public void CalculateMaxLucidity_ShouldReturnPow(int pow, int expectedLucidity)
    {
        var result = InvestigatorCalculator.CalculateMaxLucidity(pow);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedLucidity, result.Value);
    }

    [Fact]
    public void CalculateMaxLucidity_NegativePow_ReturnsFailure()
    {
        var result = InvestigatorCalculator.CalculateMaxLucidity(-5);
        Assert.True(result.IsFailure);
        Assert.Equal("O atributo POW não pode ser negativo.", result.Error);
    }

    [Theory]
    [InlineData(50, 10)]
    [InlineData(75, 15)]
    [InlineData(14, 2)]
    [InlineData(0, 0)]
    public void CalculateMaxMp_ShouldReturnPowDividedByFive(int pow, int expectedMp)
    {
        var result = InvestigatorCalculator.CalculateMaxMp(pow);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedMp, result.Value);
    }

    [Fact]
    public void CalculateMaxMp_NegativePow_ReturnsFailure()
    {
        var result = InvestigatorCalculator.CalculateMaxMp(-1);
        Assert.True(result.IsFailure);
        Assert.Equal("O atributo POW não pode ser negativo.", result.Error);
    }

    [Theory]
    [InlineData(20, 40, "-2", -2)] // Sum 60 <= 64
    [InlineData(30, 34, "-2", -2)] // Sum 64 <= 64
    [InlineData(35, 35, "-1", -1)] // Sum 70 <= 84
    [InlineData(40, 44, "-1", -1)] // Sum 84 <= 84
    [InlineData(50, 50, "0", 0)] // Sum 100 <= 124
    [InlineData(60, 64, "0", 0)] // Sum 124 <= 124
    [InlineData(70, 80, "+1d4", 1)] // Sum 150 <= 164
    [InlineData(80, 84, "+1d4", 1)] // Sum 164 <= 164
    [InlineData(90, 90, "+1d6", 2)] // Sum 180 <= 204
    [InlineData(100, 104, "+1d6", 2)] // Sum 204 <= 204
    [InlineData(105, 105, "+2d6", 3)] // Sum 210 > 204
    public void CalculateDamageBonusAndBuild_ShouldReturnCorrectTuple(
        int str, int siz, string expectedDb, int expectedBuild
    )
    {
        var result = InvestigatorCalculator.CalculateDamageBonusAndBuild(str, siz);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDb, result.Value.DamageBonus);
        Assert.Equal(expectedBuild, result.Value.Build);
    }

    [Theory]
    [InlineData(-1, 50)]
    [InlineData(50, -10)]
    public void CalculateDamageBonusAndBuild_NegativeInputs_ReturnsFailure(int str, int siz)
    {
        var result = InvestigatorCalculator.CalculateDamageBonusAndBuild(str, siz);
        Assert.True(result.IsFailure);
        Assert.Equal("Os atributos STR e SIZ não podem ser negativos.", result.Error);
    }

    [Theory]
    // dex < siz && str < siz (base 7)
    [InlineData(40, 50, 60, 25, 7)]
    [InlineData(40, 50, 60, 45, 6)] // age 45 -> -1 (7-1=6)
    [InlineData(40, 50, 60, 55, 5)] // age 55 -> -2 (7-2=5)
    [InlineData(40, 50, 60, 65, 4)] // age 65 -> -3 (7-3=4)
    [InlineData(40, 50, 60, 75, 3)] // age 75 -> -4 (7-4=3)
    [InlineData(40, 50, 60, 85, 2)] // age 85 -> -5 (7-5=2)
    [InlineData(40, 50, 60, 110, 1)] // clamped at minimum 1
    // str > siz && dex > siz (base 9)
    [InlineData(70, 70, 50, 25, 9)]
    [InlineData(70, 70, 50, 40, 8)] // age 40 -> -1 (9-1=8)
    // str > siz && dex <= siz (base 8)
    [InlineData(70, 50, 60, 30, 8)]
    // dex > siz && str <= siz (base 8)
    [InlineData(50, 70, 60, 30, 8)]
    // str == siz && dex == siz (base 8)
    [InlineData(60, 60, 60, 30, 8)]
    public void CalculateMovementRate_ShouldAccountForStatsAndAge(
        int str, int dex, int siz, int age, int expectedMov
    )
    {
        var result = InvestigatorCalculator.CalculateMovementRate(str, dex, siz, age);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedMov, result.Value);
    }

    [Theory]
    [InlineData(-1, 50, 50, 30)]
    [InlineData(50, -1, 50, 30)]
    [InlineData(50, 50, -1, 30)]
    [InlineData(50, 50, 50, -1)]
    public void CalculateMovementRate_NegativeInputs_ReturnsFailure(int str, int dex, int siz, int age)
    {
        var result = InvestigatorCalculator.CalculateMovementRate(str, dex, siz, age);
        Assert.True(result.IsFailure);
        Assert.Equal("Atributos e idade não podem ser negativos.", result.Error);
    }
}