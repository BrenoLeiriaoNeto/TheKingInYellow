using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class DiceRollerTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(100)]
    public void Roll_ValidSides_ReturnsValueWithinRange(int sides)
    {
        for (var i = 0; i < 100; i++)
        {
            var roll = DiceRoller.Roll(sides);
            Assert.InRange(roll, 1, sides);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Roll_InvalidSides_ThrowsArgumentOutOfRangeException(int invalidSides)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => DiceRoller.Roll(invalidSides));
        Assert.Equal("sides", ex.ParamName);
    }

    [Theory]
    [InlineData(1, 6, 0)]
    [InlineData(3, 6, 2)]
    [InlineData(2, 10, -3)]
    [InlineData(5, 20, 10)]
    public void RollDice_ValidParameters_ReturnsCorrectResultStructureAndTotal(int count, int sides,
        int modifier)
    {
        var result = DiceRoller.RollDice(count, sides, modifier);

        Assert.Equal(count, result.DiceCount);
        Assert.Equal(sides, result.Sides);
        Assert.Equal(modifier, result.Modifier);
        Assert.Equal(count, result.IndividualRolls.Count);

        var sum = 0;
        foreach (var roll in result.IndividualRolls)
        {
            Assert.InRange(roll, 1, sides);
            sum += roll;
        }

        Assert.Equal(sum + modifier, result.Total);
    }

    [Theory]
    [InlineData(0, 6)]
    [InlineData(-1, 6)]
    public void RollDice_InvalidCount_ThrowsArgumentOutOfRangeException(int invalidCount, int sides)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            DiceRoller.RollDice(invalidCount, sides));
        Assert.Equal("count", ex.ParamName);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, -5)]
    public void RollDice_InvalidSides_ThrowsArgumentOutOfRangeException(int count, int invalidSides)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            DiceRoller.RollDice(count, invalidSides));
        Assert.Equal("sides", ex.ParamName);
    }

    [Fact]
    public void RollD3_ReturnsValueBetween1And3()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD3();
            Assert.InRange(roll, 1, 3);
        }
    }

    [Fact]
    public void RollD4_ReturnsValueBetween1And4()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD4();
            Assert.InRange(roll, 1, 4);
        }
    }

    [Fact]
    public void RollD6_ReturnsValueBetween1And6()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD6();
            Assert.InRange(roll, 1, 6);
        }
    }

    [Fact]
    public void RollD8_ReturnsValueBetween1And8()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD8();
            Assert.InRange(roll, 1, 8);
        }
    }

    [Fact]
    public void RollD10_ReturnsValueBetween1And10()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD10();
            Assert.InRange(roll, 1, 10);
        }
    }

    [Fact]
    public void RollD20_ReturnsValueBetween1And20()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD20();
            Assert.InRange(roll, 1, 20);
        }
    }

    [Fact]
    public void RollD100_ReturnsValueBetween1And100()
    {
        for (var i = 0; i < 50; i++)
        {
            var roll = DiceRoller.RollD100();
            Assert.InRange(roll, 1, 100);
        }
    }

    [Fact]
    public void RollAttribute3D6_ReturnsMultipleOf5WithinExpectedRange()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = DiceRoller.RollAttribute3D6();
            // 3d6 has range 3..18, multiplied by 5 => 15..90
            Assert.InRange(value, 15, 90);
            Assert.Equal(0, value % 5);
        }
    }

    [Fact]
    public void RollAttribute2D6Plus6_ReturnsMultipleOf5WithinExpectedRange()
    {
        for (var i = 0; i < 100; i++)
        {
            var value = DiceRoller.RollAttribute2D6Plus6();
            // 2d6+6 has range 8..18, multiplied by 5 => 40..90
            Assert.InRange(value, 40, 90);
            Assert.Equal(0, value % 5);
        }
    }

    [Fact]
    public void RandomDiceRoller_ImplementsIDiceRollerAndReturnsValuesInRange()
    {
        var roller = new TheKingInYellow.Core.Interfaces.RandomDiceRoller();

        Assert.InRange(roller.RollD100(), 1, 100);
        Assert.InRange(roller.RollD20(), 1, 20);
        Assert.InRange(roller.RollD12(), 1, 12);
        Assert.InRange(roller.RollD10(), 1, 10);
        Assert.InRange(roller.RollD8(), 1, 8);
        Assert.InRange(roller.RollD6(), 1, 6);
        Assert.InRange(roller.RollD4(), 1, 4);
        Assert.InRange(roller.RollD3(), 1, 3);
    }

    [Fact]
    public void FixedDiceRoller_ReturnsSpecifiedFixedValues()
    {
        var roller = new TheKingInYellow.Core.Interfaces.FixedDiceRoller(
            fixedD100: 42,
            fixedD20: 17,
            fixedD12: 11,
            fixedD10: 9,
            fixedD8: 7,
            fixedD6: 5,
            fixedD4: 3,
            fixedD3: 2
        );

        Assert.Equal(42, roller.RollD100());
        Assert.Equal(17, roller.RollD20());
        Assert.Equal(11, roller.RollD12());
        Assert.Equal(9, roller.RollD10());
        Assert.Equal(7, roller.RollD8());
        Assert.Equal(5, roller.RollD6());
        Assert.Equal(3, roller.RollD4());
        Assert.Equal(2, roller.RollD3());
    }
}