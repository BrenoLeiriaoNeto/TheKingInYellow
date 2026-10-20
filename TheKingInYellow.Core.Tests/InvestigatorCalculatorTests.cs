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
        var actualHp = InvestigatorCalculator.CalculateMaxHp(con, siz);
        Assert.Equal(expectedHp, actualHp);
    }

    [Theory]
    [InlineData(50, 50)]
    [InlineData(85, 85)]
    [InlineData(0, 0)]
    public void CalculateMaxLucidity_ShouldReturnPow(int pow, int expectedLucidity)
    {
        var actual = InvestigatorCalculator.CalculateMaxLucidity(pow);
        Assert.Equal(expectedLucidity, actual);
    }

    [Theory]
    [InlineData(50, 10)]
    [InlineData(75, 15)]
    [InlineData(14, 2)]
    [InlineData(0, 0)]
    public void CalculateMaxMp_ShouldReturnPowDividedByFive(int pow, int expectedMp)
    {
        var actual = InvestigatorCalculator.CalculateMaxMp(pow);
        Assert.Equal(expectedMp, actual);
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
        var (actualDb, actualBuild) = InvestigatorCalculator.CalculateDamageBonusAndBuild(str, siz);
        Assert.Equal(expectedDb, actualDb);
        Assert.Equal(expectedBuild, actualBuild);
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
        var actualMov = InvestigatorCalculator.CalculateMovementRate(str, dex, siz, age);
        Assert.Equal(expectedMov, actualMov);
    }
}