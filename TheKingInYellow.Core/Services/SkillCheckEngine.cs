using TheKingInYellow.Core.Domain.Enums;
using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

public static class SkillCheckEngine
{
    public static SkillCheckResult RollCheck(int skillValue, int bonusPenaltyDice = 0)
    {
        int finalRoll = RollD100WithBonusPenalty(bonusPenaltyDice);
        SuccessLevel level = EvaluateRoll(finalRoll, skillValue);
        
        return new SkillCheckResult(
            RollValue: finalRoll,
            TargetValue: skillValue,
            Level: level,
            IsSuccess: level >= SuccessLevel.Regular,
            BonusPenaltyDice: bonusPenaltyDice);
    }

    public static SuccessLevel EvaluateRoll(int roll, int skillValue)
    {

        bool isFumble = skillValue < 50 ? roll >= 96 : roll == 100;
        if (isFumble) return SuccessLevel.Fumble;

        if (roll == 1) return SuccessLevel.Critical;

        if (roll > skillValue) return SuccessLevel.Failure;

        int extremeTarget = skillValue / 5;
        int hardTarget = skillValue / 2;
        
        if (roll <= extremeTarget) return SuccessLevel.Extreme;
        return roll <= hardTarget ? SuccessLevel.Hard : SuccessLevel.Regular;
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