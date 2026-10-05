namespace TheKingInYellow.Core.Domain.ValueObjects;
/// <summary>
/// Estrutura imutável que representa o resultado detalhado de uma rolagem de dados.
/// </summary>
/// <param name="Total"></param>
/// <param name="IndividualRolls"></param>
/// <param name="DiceCount"></param>
/// <param name="Sides"></param>
/// <param name="Modifier"></param>
public readonly record struct DiceRollResult(
    int Total,
    IReadOnlyList<int> IndividualRolls,
    int DiceCount,
    int Sides,
    int Modifier
    );