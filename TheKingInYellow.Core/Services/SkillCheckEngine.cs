using TheKingInYellow.Core.Domain.Enums;
using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

public static class SkillCheckEngine
{
    public static SkillCheckResult RollCheck(int skillValue, int bonusPenaltyDice = 0)
    {
        var finalRoll = RollD100WithBonusPenalty(bonusPenaltyDice);
        var level = EvaluateRoll(finalRoll, skillValue);
        
        return new SkillCheckResult(
            RollValue: finalRoll,
            TargetValue: skillValue,
            Level: level,
            IsSuccess: level >= SuccessLevel.Regular,
            BonusPenaltyDice: bonusPenaltyDice);
    }

    public static SuccessLevel EvaluateRoll(int roll, int skillValue)
    {

        var isFumble = skillValue < 50 ? roll >= 96 : roll == 100;
        if (isFumble) return SuccessLevel.Fumble;

        if (roll == 1) return SuccessLevel.Critical;

        if (roll > skillValue) return SuccessLevel.Failure;

        var extremeTarget = skillValue / 5;
        var hardTarget = skillValue / 2;
        
        if (roll <= extremeTarget) return SuccessLevel.Extreme;
        return roll <= hardTarget ? SuccessLevel.Hard : SuccessLevel.Regular;
    }

    public static int ResolveBaseValue(SkillDefinition skill, int dexterity)
    {
        if (skill.DerivedAttribute == "DEX_DIV_2") return dexterity / 2;

        return skill.BaseValue;
    }

    private static int RollD100WithBonusPenalty(int bonusPenaltyDice)
    {
        if (bonusPenaltyDice == 0) return DiceRoller.RollD100();

        var unitDigit = Random.Shared.Next(0, 10);
        var tensCount = 1 + Math.Abs(bonusPenaltyDice);
        var tensRolls = new int[tensCount];

        for (var i = 0; i < tensCount; i++)
        {
            tensRolls[i] = Random.Shared.Next(0, 10) * 10;
        }
        
        var chosenTens = bonusPenaltyDice > 0 ? tensRolls.Min() : tensRolls.Max();
        var total = chosenTens + unitDigit;

        return total == 0 ? 100 : total;
    }
}