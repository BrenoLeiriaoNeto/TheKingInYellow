using TheKingInYellow.Core.Domain.Enums;
using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Motor de regras responsável pelo processamento de testes de perícia (Skill Checks) e resolução de valores derivados.
/// </summary>
public static class SkillCheckEngine
{
    /// <summary>
    /// Realiza um teste de perícia rolando um dado percentual (d100) com eventuais dados de bônus ou penalidade,
    /// e avalia o resultado frente ao valor alvo da perícia.
    /// </summary>
    /// <param name="skillValue">O valor percentual da perícia do personagem (alvo do teste).</param>
    /// <param name="bonusPenaltyDice">A quantidade de dados extras de dezena: valores positivos indicam bônus e valores negativos indicam penalidade.</param>
    /// <returns>Uma estrutura <see cref="SkillCheckResult"/> contendo os detalhes e o resultado consolidado do teste.</returns>
    public static SkillCheckResult RollCheck(int skillValue, int bonusPenaltyDice = 0)
    {
       return RollTaskCheck(
            skillValue,
            SuccessLevel.Regular,
            bonusPenaltyDice);
    }

    /// <summary>
    /// Avalia o valor de uma rolagem em relação ao valor da perícia, determinando o nível de sucesso ou falha obtido.
    /// </summary>
    /// <param name="roll">O valor obtido na rolagem do d100 (1 a 100).</param>
    /// <param name="skillValue">O valor da perícia do personagem.</param>
    /// <returns>O nível de sucesso correspondente (<see cref="SuccessLevel"/>).</returns>
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

    /// <summary>
    /// Resolve o valor base de uma perícia, aplicando regras de atributos derivados quando configurado.
    /// </summary>
    /// <param name="skill">A definição da perícia a ser calculada.</param>
    /// <param name="dexterity">O valor do atributo Destreza (DEX) do personagem.</param>
    /// <returns>O valor base calculado da perícia.</returns>
    public static int ResolveBaseValue(SkillDefinition skill, int dexterity)
    {
        if (skill.DerivedAttribute == "DEX_DIV_2") return dexterity / 2;

        return skill.BaseValue;
    }

    /// <summary>
    /// Rola um d100 aplicando a mecânica de dados de bônus ou penalidade sobre o dígito das dezenas.
    /// </summary>
    /// <param name="bonusPenaltyDice">A quantidade de dados extras de dezena a rolar (positivo para bônus, negativo para penalidade).</param>
    /// <returns>O valor numérico final entre 1 e 100.</returns>
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

    /// <summary>
    /// Determina se o nível de sucesso atingido é suficiente para cumprir o nível exigido pela tarefa.
    /// Falhas normais e desastres (fumble) sempre resultam em insucesso.
    /// </summary>
    /// <param name="achievedLevel">O nível de sucesso obtido na rolagem.</param>
    /// <param name="requiredLevel">O nível mínimo de sucesso exigido pela tarefa (padrão é Regular).</param>
    /// <returns><c>true</c> se o nível obtido for igual ou superior ao nível exigido; caso contrário, <c>false</c>.</returns>
    public static bool IsTaskSuccessful(SuccessLevel achievedLevel,
        SuccessLevel requiredLevel = SuccessLevel.Regular)
    {
        if (achievedLevel is SuccessLevel.Failure or SuccessLevel.Fumble)
            return false;

        return achievedLevel >= requiredLevel;
    }

    /// <summary>
    /// Realiza um teste de tarefa com nível de dificuldade exigido e dados de bônus/penalidade opcionais,
    /// avaliando a rolagem e determinando se a tarefa foi completada com sucesso.
    /// </summary>
    /// <param name="skillValue">O valor percentual da perícia do personagem (alvo do teste).</param>
    /// <param name="requiredLevel">O nível mínimo de sucesso exigido para a tarefa.</param>
    /// <param name="bonusPenalty">A quantidade de dados extras de dezena: valores positivos indicam bônus e valores negativos indicam penalidade.</param>
    /// <returns>Uma estrutura <see cref="SkillCheckResult"/> contendo a rolagem, nível obtido, exigência e se foi bem-sucedida.</returns>
    public static SkillCheckResult RollTaskCheck(int skillValue, SuccessLevel requiredLevel,
        int bonusPenalty = 0)
    {
        var finalRoll = RollD100WithBonusPenalty(bonusPenalty);
        var achievedLevel = EvaluateRoll(finalRoll, skillValue);
        var isSuccess = IsTaskSuccessful(achievedLevel, requiredLevel);

        return new SkillCheckResult(
            RollValue: finalRoll,
            TargetValue: skillValue,
            Level: achievedLevel,
            IsSuccess: isSuccess,
            BonusPenaltyDice: bonusPenalty,
            RequiredLevel: requiredLevel);
    }
}