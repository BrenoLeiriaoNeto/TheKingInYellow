namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Estrutura imutável que representa o resultado detalhado de uma rolagem de dados.
/// </summary>
/// <param name="Total">O total consolidado da rolagem (soma dos dados somada ao modificador).</param>
/// <param name="IndividualRolls">A lista contendo o valor individual obtido em cada dado rolado.</param>
/// <param name="DiceCount">A quantidade de dados rolados.</param>
/// <param name="Sides">A quantidade de lados/faces de cada dado rolado.</param>
/// <param name="Modifier">O valor modificador aplicado à soma dos dados.</param>
public readonly record struct DiceRollResult(
    int Total,
    IReadOnlyList<int> IndividualRolls,
    int DiceCount,
    int Sides,
    int Modifier
    );