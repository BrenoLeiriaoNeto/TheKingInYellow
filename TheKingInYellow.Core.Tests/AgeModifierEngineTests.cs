using TheKingInYellow.Core.Interfaces;
using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class AgeModifierEngineTests
{
    [Fact]
    public void EduImprovementCheck_ShouldIncreaseEdu_WhenRollIsGreaterThanCurrentEdu()
    {
        var diceRoller = new FixedDiceRoller(fixedD100: 80, fixedD10: 6);

        var updatedEdu = AgeModifierEngine.PerformEduImprovementCheck(60, diceRoller);

        Assert.Equal(66, updatedEdu);
    }

    [Fact]
    public void EduImprovementCheck_ShouldNotIncreaseEdu_WhenRollIsLessThanCurrentEdu()
    {
        var diceRoller = new FixedDiceRoller(fixedD100: 20);

        var updatedEdu = AgeModifierEngine.PerformEduImprovementCheck(60, diceRoller);

        Assert.Equal(60, updatedEdu);
    }

    [Fact]
    public void EduImprovementCheck_ShouldIncreaseEdu_WhenRollIsGreaterThan95EvenIfLowerThanEdu()
    {
        // When EDU is 98 and roll is 96, roll <= currentEdu is true, but roll <= 95 is false -> improvement occurs!
        var diceRoller = new FixedDiceRoller(fixedD100: 96, fixedD10: 5);

        var updatedEdu = AgeModifierEngine.PerformEduImprovementCheck(98, diceRoller);

        // Clamped at 99 (98 + 5 = 103 -> 99)
        Assert.Equal(99, updatedEdu);
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldDeductStrAndEdu_WhenAgeIsYoungerThan20()
    {
        var diceRoller = new FixedDiceRoller();

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 18,
            str: 50,
            con: 50,
            dex: 50,
            app: 50,
            edu: 60,
            siz: 50,
            diceRoller
        );

        Assert.Equal(45, result.Str);
        Assert.Equal(55, result.Edu);
        Assert.Equal(50, result.Con);
        Assert.Equal(50, result.Dex);
        Assert.Equal(50, result.App);
        Assert.Equal(50, result.Siz);
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldClampAt15_WhenYoungAgeDeductionGoesLow()
    {
        var diceRoller = new FixedDiceRoller();

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 16,
            str: 18,
            con: 50,
            dex: 50,
            app: 50,
            edu: 17,
            siz: 50,
            diceRoller
        );

        Assert.Equal(15, result.Str); // 18 - 5 = 13 -> clamped at 15
        Assert.Equal(15, result.Edu); // 17 - 5 = 12 -> clamped at 15
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldPerformSingleEduCheck_WhenAgeBetween20And39()
    {
        var diceRoller = new FixedDiceRoller(fixedD100: 80, fixedD10: 7);

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 30,
            str: 60,
            con: 50,
            dex: 50,
            app: 60,
            edu: 50,
            siz: 60,
            diceRoller
        );

        Assert.Equal(57, result.Edu); // EDU increased by 7
        Assert.Equal(60, result.Str); // No physical deduction
        Assert.Equal(60, result.App); // No APP deduction
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldDeductPhysicalAndApp_WhenAgeIs45()
    {
        var diceRoller =
            new FixedDiceRoller(fixedD100: 10); // EDU check will pass/fail without increase

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 45,
            str: 60,
            con: 50,
            dex: 50,
            app: 60,
            edu: 50,
            siz: 60,
            diceRoller
        );

        Assert.Equal(55, result.App); // -5 APP
        Assert.Equal(55, result.Str); // -5 physical from STR (highest)
        Assert.Equal(50, result.Con);
        Assert.Equal(50, result.Dex);
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldApplyDeductionsAndPerform3EduChecks_WhenAgeIs50()
    {
        // 3 checks with roll 80 and +2 each time
        var diceRoller = new FixedDiceRoller(fixedD100: 80, fixedD10: 2);

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 55,
            str: 60,
            con: 60,
            dex: 50,
            app: 60,
            edu: 50,
            siz: 60,
            diceRoller
        );

        // EDU gets 3 checks: 50 -> 52 -> 54 -> 56
        Assert.Equal(56, result.Edu);
        // APP deduction: 10 (age <= 59)
        Assert.Equal(50, result.App);
        // Physical deduction: 10 (STR was 60 >= CON 60 -> STR -5 to 55, then CON 60 > STR 55 -> CON -5 to 55)
        Assert.Equal(55, result.Str);
        Assert.Equal(55, result.Con);
        Assert.Equal(50, result.Dex);
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldApplyDeductionsAndPerform4EduChecks_WhenAgeIs65()
    {
        var diceRoller = new FixedDiceRoller(fixedD100: 80, fixedD10: 1);

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 65,
            str: 60,
            con: 50,
            dex: 50,
            app: 60,
            edu: 50,
            siz: 60,
            diceRoller
        );

        // EDU gets 4 checks: 50 -> 51 -> 52 -> 53 -> 54
        Assert.Equal(54, result.Edu);
        // APP deduction: 15 (age <= 69)
        Assert.Equal(45, result.App);
        // Physical deduction: 20
        // Initial STR: 60, CON: 50, DEX: 50
        // 1: STR (60>=50,50) -> STR=55
        // 2: STR (55>=50,50) -> STR=50
        // 3: STR (50>=50,50) -> STR=45
        // 4: CON (50>=50 & 50>15) -> CON=45
        Assert.Equal(45, result.Str);
        Assert.Equal(45, result.Con);
        Assert.Equal(50, result.Dex);
    }

    [Fact]
    public void ApplyAgeAdjustments_ShouldDeductCorrectAmounts_WhenAgeIs80Plus()
    {
        var diceRoller = new FixedDiceRoller(fixedD100: 10);

        var result = AgeModifierEngine.ApplyAgeAdjustments(
            age: 85,
            str: 50,
            con: 50,
            dex: 50,
            app: 30,
            edu: 50,
            siz: 50,
            diceRoller
        );

        // APP deduction 25 (30 - 25 = 5 -> clamped at 15)
        Assert.Equal(15, result.App);
        // Physical deduction: 80 -> all reduced until 15
        Assert.True(result.Str >= 15);
        Assert.True(result.Con >= 15);
        Assert.True(result.Dex >= 15);
    }
}