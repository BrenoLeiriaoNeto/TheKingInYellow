using TheKingInYellow.Core.Domain.Enums;

namespace TheKingInYellow.Core.Domain.ValueObjects;

public readonly record struct SkillCheckResult(
    int RollValue,
    int TargetValue,
    SuccessLevel Level,
    bool IsSuccess,
    int BonusPenaltyDice
    );