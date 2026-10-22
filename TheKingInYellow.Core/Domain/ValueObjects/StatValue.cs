using TheKingInYellow.Core.Domain.Primitives;

namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Estrutura imutável que encapsula um valor de atributo ou perícia, fornecendo frações calculadas (metade e um quinto).
/// </summary>
public readonly record struct StatValue
{
    
    public int Base { get; }
    
    /// <summary>
    /// O valor da metade (Regular/Difícil - Hard) calculado por divisão inteira por 2.
    /// </summary>
    public int Half => Base / 2;

    /// <summary>
    /// O valor de um quinto (Extremo - Extreme) calculado por divisão inteira por 5.
    /// </summary>
    public int Fifth => Base / 5;
    
    public StatValue(int value)
    {
        Base = Math.Clamp(value, 0, 99);
    }
    
    public static implicit operator int(StatValue statValue) => statValue.Base;
    public static implicit operator StatValue(int value) => new(value);
    
    public override string ToString() => Base.ToString();
}