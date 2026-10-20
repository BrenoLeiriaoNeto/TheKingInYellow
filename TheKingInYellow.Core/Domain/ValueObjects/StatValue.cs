namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Estrutura imutável que encapsula um valor de atributo ou perícia, fornecendo frações calculadas (metade e um quinto).
/// </summary>
/// <param name="Base">O valor base total do atributo ou perícia.</param>
public readonly record struct StatValue(int Base)
{
    /// <summary>
    /// O valor da metade (Regular/Difícil - Hard) calculado por divisão inteira por 2.
    /// </summary>
    public int Half => Base / 2;

    /// <summary>
    /// O valor de um quinto (Extremo - Extreme) calculado por divisão inteira por 5.
    /// </summary>
    public int Fifth => Base / 5;
}