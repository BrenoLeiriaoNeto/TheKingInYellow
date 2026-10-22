using TheKingInYellow.Core.Services;

namespace TheKingInYellow.Core.Interfaces;

/// <summary>
/// Interface que abstrai operações de rolagem de dados para permitir testes determinísticos e simulações.
/// </summary>
public interface IDiceRoller
{
    /// <summary>
    /// Rola um dado percentual de 100 lados (d100).
    /// </summary>
    int RollD100();

    /// <summary>
    /// Rola um dado de 20 lados (d20).
    /// </summary>
    int RollD20();

    /// <summary>
    /// Rola um dado de 12 lados (d12).
    /// </summary>
    int RollD12();

    /// <summary>
    /// Rola um dado de 10 lados (d10).
    /// </summary>
    int RollD10();

    /// <summary>
    /// Rola um dado de 8 lados (d8).
    /// </summary>
    int RollD8();

    /// <summary>
    /// Rola um dado de 6 lados (d6).
    /// </summary>
    int RollD6();

    /// <summary>
    /// Rola um dado de 4 lados (d4).
    /// </summary>
    int RollD4();

    /// <summary>
    /// Rola um dado de 3 lados (d3).
    /// </summary>
    int RollD3();
}

/// <summary>
/// Implementação padrão de <see cref="IDiceRoller"/> baseada no gerador pseudoaleatório de <see cref="DiceRoller"/>.
/// </summary>
public class RandomDiceRoller : IDiceRoller
{
    /// <inheritdoc />
    public int RollD100() => DiceRoller.RollD100();

    /// <inheritdoc />
    public int RollD20() => DiceRoller.RollD20();

    /// <inheritdoc />
    public int RollD12() => DiceRoller.RollD12();

    /// <inheritdoc />
    public int RollD10() => DiceRoller.RollD10();

    /// <inheritdoc />
    public int RollD8() => DiceRoller.RollD8();

    /// <inheritdoc />
    public int RollD6() => DiceRoller.RollD6();

    /// <inheritdoc />
    public int RollD4() => DiceRoller.RollD4();

    /// <inheritdoc />
    public int RollD3() => DiceRoller.RollD3();
}

/// <summary>
/// Implementação determinística de <see cref="IDiceRoller"/> para testes unitários com valores pré-fixados.
/// </summary>
public class FixedDiceRoller(
    int fixedD100 = 1,
    int fixedD20 = 1,
    int fixedD12 = 1,
    int fixedD10 = 1,
    int fixedD8 = 1,
    int fixedD6 = 1,
    int fixedD4 = 1,
    int fixedD3 = 1)
    : IDiceRoller
{
    /// <inheritdoc />
    public int RollD100() => fixedD100;

    /// <inheritdoc />
    public int RollD20() => fixedD20;

    /// <inheritdoc />
    public int RollD12() => fixedD12;

    /// <inheritdoc />
    public int RollD10() => fixedD10;

    /// <inheritdoc />
    public int RollD8() => fixedD8;

    /// <inheritdoc />
    public int RollD6() => fixedD6;

    /// <inheritdoc />
    public int RollD4() => fixedD4;

    /// <inheritdoc />
    public int RollD3() => fixedD3;
}