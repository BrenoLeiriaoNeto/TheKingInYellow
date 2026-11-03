using TheKingInYellow.Core.Domain.Enums;
using TheKingInYellow.Core.Domain.ValueObjects;
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
    [InlineData(50, 50, SuccessLevel.Regular)]
    [InlineData(51, 50, SuccessLevel.Failure)]
    [InlineData(95, 50, SuccessLevel.Failure)]
    [InlineData(96, 50, SuccessLevel.Failure)]
    [InlineData(99, 50, SuccessLevel.Failure)]
    [InlineData(100, 50, SuccessLevel.Fumble)]
    public void EvaluateRoll_WithNormalSkill_MustReturnCorrectLevel(int roll, int skillValue,
        SuccessLevel expected)
    {
        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData(96)]
    [InlineData(97)]
    [InlineData(98)]
    [InlineData(99)]
    [InlineData(100)]
    public void EvaluateRoll_SkillLessThan50_Rolls96To100_MustBeFumble(int roll)
    {
        var skillValue = 40;

        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessLevel.Fumble, result.Value);
    }

    [Theory]
    [InlineData(96)]
    [InlineData(97)]
    [InlineData(98)]
    [InlineData(99)]
    public void EvaluateRoll_SkillGreaterThanOrEquals50_Rolls96To99_MustBeFailure(int roll)
    {
        var skillValue = 60;

        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessLevel.Failure, result.Value);
    }

    [Fact]
    public void EvaluateRoll_SkillGreaterThanOrEquals50_Rolls100_MustBeFumble()
    {
        var skillValue = 60;
        var roll = 100;

        var result = SkillCheckEngine.EvaluateRoll(roll, skillValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(SuccessLevel.Fumble, result.Value);
    }

    [Theory]
    [InlineData(0, 50)]
    [InlineData(101, 50)]
    [InlineData(-5, 50)]
    public void EvaluateRoll_InvalidRoll_ReturnsFailure(int invalidRoll, int skillValue)
    {
        var result = SkillCheckEngine.EvaluateRoll(invalidRoll, skillValue);

        Assert.True(result.IsFailure);
        Assert.Equal("O valor da rolagem de d100 deve estar entre 1 e 100.", result.Error);
    }

    [Fact]
    public void EvaluateRoll_NegativeSkillValue_ReturnsFailure()
    {
        var result = SkillCheckEngine.EvaluateRoll(50, -10);

        Assert.True(result.IsFailure);
        Assert.Equal("O valor da perícia não pode ser negativo.", result.Error);
    }

    [Fact]
    public void RollCheck_WithBonusDice_MustSelectTheLesser()
    {
        for (var i = 0; i < 100; i++)
        {
            var result = SkillCheckEngine.RollCheck(50, 1);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.RollValue is >= 1 and <= 100);
            Assert.Equal(1, result.Value.BonusPenaltyDice);
            Assert.Equal(50, result.Value.TargetValue);
            Assert.Equal(result.Value.Level >= SuccessLevel.Regular, result.Value.IsSuccess);
        }
    }

    [Fact]
    public void RollCheck_WithPenaltyDice_MustExecuteSuccessfully()
    {
        for (var i = 0; i < 100; i++)
        {
            var result = SkillCheckEngine.RollCheck(50, -1);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.RollValue is >= 1 and <= 100);
            Assert.Equal(-1, result.Value.BonusPenaltyDice);
            Assert.Equal(50, result.Value.TargetValue);
            Assert.Equal(result.Value.Level >= SuccessLevel.Regular, result.Value.IsSuccess);
        }
    }

    [Fact]
    public void RollCheck_WithoutBonusOrPenalty_MustExecuteSuccessfully()
    {
        for (var i = 0; i < 50; i++)
        {
            var result = SkillCheckEngine.RollCheck(60, 0);

            Assert.True(result.IsSuccess);
            Assert.True(result.Value.RollValue is >= 1 and <= 100);
            Assert.Equal(0, result.Value.BonusPenaltyDice);
            Assert.Equal(60, result.Value.TargetValue);
            Assert.Equal(result.Value.Level >= SuccessLevel.Regular, result.Value.IsSuccess);
        }
    }

    [Theory]
    [InlineData(50, 25)]
    [InlineData(51, 25)]
    [InlineData(80, 40)]
    [InlineData(0, 0)]
    public void ResolveBaseValue_DerivedAttributeDexDiv2_ReturnsHalfDexterity(int dexterity, int expectedValue)
    {
        var skill = new SkillDefinition("dodge", "Dodge", 0, "Physical", "", DerivedAttribute: "DEX_DIV_2");

        var result = SkillCheckEngine.ResolveBaseValue(skill, dexterity);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
    }

    [Fact]
    public void ResolveBaseValue_FixedBaseValue_ReturnsStaticBaseValue()
    {
        var skill = new SkillDefinition("spot_hidden", "Spot Hidden", 25, "Awareness", "", DerivedAttribute: null);

        var result = SkillCheckEngine.ResolveBaseValue(skill, dexterity: 80);

        Assert.True(result.IsSuccess);
        Assert.Equal(25, result.Value);
    }

    [Fact]
    public void ResolveBaseValue_NegativeDexterity_ReturnsFailure()
    {
        var skill = new SkillDefinition("dodge", "Dodge", 0, "Physical", "", DerivedAttribute: "DEX_DIV_2");

        var result = SkillCheckEngine.ResolveBaseValue(skill, dexterity: -10);

        Assert.True(result.IsFailure);
        Assert.Equal("O valor de Destreza não pode ser negativo.", result.Error);
    }

    [Theory]
    [InlineData(SuccessLevel.Fumble, SuccessLevel.Regular, false)]
    [InlineData(SuccessLevel.Fumble, SuccessLevel.Hard, false)]
    [InlineData(SuccessLevel.Failure, SuccessLevel.Regular, false)]
    [InlineData(SuccessLevel.Failure, SuccessLevel.Hard, false)]
    [InlineData(SuccessLevel.Regular, SuccessLevel.Regular, true)]
    [InlineData(SuccessLevel.Regular, SuccessLevel.Hard, false)]
    [InlineData(SuccessLevel.Regular, SuccessLevel.Extreme, false)]
    [InlineData(SuccessLevel.Hard, SuccessLevel.Regular, true)]
    [InlineData(SuccessLevel.Hard, SuccessLevel.Hard, true)]
    [InlineData(SuccessLevel.Hard, SuccessLevel.Extreme, false)]
    [InlineData(SuccessLevel.Extreme, SuccessLevel.Regular, true)]
    [InlineData(SuccessLevel.Extreme, SuccessLevel.Hard, true)]
    [InlineData(SuccessLevel.Extreme, SuccessLevel.Extreme, true)]
    [InlineData(SuccessLevel.Extreme, SuccessLevel.Critical, false)]
    [InlineData(SuccessLevel.Critical, SuccessLevel.Regular, true)]
    [InlineData(SuccessLevel.Critical, SuccessLevel.Hard, true)]
    [InlineData(SuccessLevel.Critical, SuccessLevel.Extreme, true)]
    [InlineData(SuccessLevel.Critical, SuccessLevel.Critical, true)]
    public void IsTaskSuccessful_EvaluatesAchievedVersusRequiredCorrectly(
        SuccessLevel achieved, SuccessLevel required, bool expectedSuccess)
    {
        var isSuccess = SkillCheckEngine.IsTaskSuccessful(achieved, required);

        Assert.Equal(expectedSuccess, isSuccess);
    }

    [Theory]
    [InlineData(SuccessLevel.Regular)]
    [InlineData(SuccessLevel.Hard)]
    [InlineData(SuccessLevel.Extreme)]
    public void RollTaskCheck_ReturnsConsistentSkillCheckResult(SuccessLevel requiredLevel)
    {
        for (var i = 0; i < 50; i++)
        {
            var result = SkillCheckEngine.RollTaskCheck(60, requiredLevel, bonusPenalty: 0);

            Assert.True(result.IsSuccess);
            Assert.InRange(result.Value.RollValue, 1, 100);
            Assert.Equal(60, result.Value.TargetValue);
            Assert.Equal(0, result.Value.BonusPenaltyDice);
            Assert.Equal(requiredLevel, result.Value.RequiredLevel);

            var expectedSuccess = SkillCheckEngine.IsTaskSuccessful(result.Value.Level, requiredLevel);
            Assert.Equal(expectedSuccess, result.Value.IsSuccess);
        }
    }

    [Fact]
    public void RollTaskCheck_WithBonusAndPenaltyDice_AppliesModifierCorrectly()
    {
        var bonusResult = SkillCheckEngine.RollTaskCheck(50, SuccessLevel.Hard, bonusPenalty: 1);
        Assert.True(bonusResult.IsSuccess);
        Assert.Equal(1, bonusResult.Value.BonusPenaltyDice);
        Assert.Equal(SuccessLevel.Hard, bonusResult.Value.RequiredLevel);

        var penaltyResult = SkillCheckEngine.RollTaskCheck(50, SuccessLevel.Regular, bonusPenalty: -1);
        Assert.True(penaltyResult.IsSuccess);
        Assert.Equal(-1, penaltyResult.Value.BonusPenaltyDice);
        Assert.Equal(SuccessLevel.Regular, penaltyResult.Value.RequiredLevel);
    }

    [Fact]
    public void RollTaskCheck_NegativeSkillValue_ReturnsFailure()
    {
        var result = SkillCheckEngine.RollTaskCheck(-10, SuccessLevel.Regular);

        Assert.True(result.IsFailure);
        Assert.Equal("O valor da perícia não pode ser negativo.", result.Error);
    }
}