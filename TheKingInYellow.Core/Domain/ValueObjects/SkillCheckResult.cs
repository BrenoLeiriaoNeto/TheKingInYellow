using TheKingInYellow.Core.Domain.Enums;

namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Estrutura imutável que representa o resultado detalhado de um teste de perícia (Skill Check).
/// </summary>
/// <param name="RollValue">O valor numérico final obtido na rolagem de d100.</param>
/// <param name="TargetValue">O valor alvo da perícia do personagem utilizado como referência para o teste.</param>
/// <param name="Level">O nível de sucesso ou falha obtido no teste.</param>
/// <param name="IsSuccess">Indica se o teste resultou em sucesso (nível Regular ou superior).</param>
/// <param name="BonusPenaltyDice">A quantidade de dados de bônus (positivo) ou penalidade (negativo) aplicados no teste.</param>
public readonly record struct SkillCheckResult(
    int RollValue,
    int TargetValue,
    SuccessLevel Level,
    bool IsSuccess,
    int BonusPenaltyDice = 0,
    SuccessLevel RequiredLevel = SuccessLevel.Regular
    );