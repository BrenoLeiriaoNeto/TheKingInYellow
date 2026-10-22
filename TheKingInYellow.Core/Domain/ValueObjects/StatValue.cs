using TheKingInYellow.Core.Domain.Primitives;

namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Estrutura imutável que encapsula um valor de atributo ou perícia, fornecendo frações calculadas (metade e um quinto).
/// </summary>
public readonly record struct StatValue
{
    /// <summary>
    /// O valor base do atributo ou perícia, limitado ao intervalo de 0 a 99.
    /// </summary>
    public int Base { get; }
    
    /// <summary>
    /// O valor da metade (Regular/Difícil - Hard) calculado por divisão inteira por 2.
    /// </summary>
    public int Half => Base / 2;

    /// <summary>
    /// O valor de um quinto (Extremo - Extreme) calculado por divisão inteira por 5.
    /// </summary>
    public int Fifth => Base / 5;
    
    /// <summary>
    /// Inicializa uma nova instância de <see cref="StatValue"/> com o valor base informado, ajustando-o para o intervalo [0, 99].
    /// </summary>
    /// <param name="value">O valor numérico do atributo ou perícia.</param>
    public StatValue(int value)
    {
        Base = Math.Clamp(value, 0, 99);
    }
    
    /// <summary>
    /// Converte implicitamente uma instância de <see cref="StatValue"/> para o seu valor inteiro base.
    /// </summary>
    /// <param name="statValue">A estrutura <see cref="StatValue"/> a ser convertida.</param>
    public static implicit operator int(StatValue statValue) => statValue.Base;

    /// <summary>
    /// Converte implicitamente um número inteiro em uma nova instância de <see cref="StatValue"/>.
    /// </summary>
    /// <param name="value">O valor inteiro base.</param>
    public static implicit operator StatValue(int value) => new(value);
    
    /// <summary>
    /// Retorna a representação textual do valor base do atributo.
    /// </summary>
    /// <returns>Uma cadeia de caracteres representando o valor base.</returns>
    public override string ToString() => Base.ToString();
}