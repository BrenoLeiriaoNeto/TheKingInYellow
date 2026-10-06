using TheKingInYellow.Core.Domain.Enums;
using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class SkillCheckEngineTests
{
    [Theory]
    [InlineData(1, 50, SuccessLevel.Critical)]
    [InlineData(10, 50, SuccessLevel.Extreme)]
    [InlineData(25, 50, SuccessLevel.Hard)]
    [InlineData(45, 50, SuccessLevel.Regular)]
    [InlineData(51, 50, SuccessLevel.Failure)]
    public void EvaluateRoll_WithNormalSkill_MustReturnCorrectLevel(int roll, int skillValue,
        SuccessLevel expected)
    {
        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void EvaluateRoll_SkillLessThan50_Rolls96_MustBeFumble()
    {
        var skillValue = 40;
        var roll = 96;
        
        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);
        
        Assert.Equal(SuccessLevel.Fumble, result);
    }
    
    [Fact]
    public void EvaluateRoll_SkillGreaterThanOrEquals50_Rolls96_MustBeFailure()
    {
        var skillValue = 60;
        var roll = 96;

        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);

        Assert.Equal(SuccessLevel.Failure, result);
    }

    [Fact]
    public void RollCheck_WithBonusDice_MustSelectTheLesser()
    {
        for (var i = 0; i < 100; i++)
        {
            var result = SkillCheckEngine.RollCheck(50, 1);

            Assert.True(result.RollValue is >= 1 and <= 100);
            Assert.Equal(1, result.BonusPenaltyDice);
        }
    }
}