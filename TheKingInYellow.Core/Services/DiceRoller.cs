using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Serviço estático utilitário responsável pela simulação e rolagem de dados no sistema.
/// </summary>
public static class DiceRoller
{
    /// <summary>
    /// Rola um único dado com a quantidade de faces/lados especificada.
    /// </summary>
    /// <param name="sides">A quantidade de lados do dado (deve ser no mínimo 1).</param>
    /// <returns>Um número inteiro pseudoaleatório entre 1 e <paramref name="sides"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Lançada quando <paramref name="sides"/> for menor que 1.</exception>
    public static int Roll(int sides)
    {
        if (sides < 1)
            throw new ArgumentOutOfRangeException(nameof(sides),
                "The dice must have at least 1 side.");

        return Random.Shared.Next(1, sides + 1);
    }

    /// <summary>
    /// Rola múltiplos dados do mesmo tipo e aplica um modificador ao total.
    /// Exemplo: 3d6 + 2 -> RollDice(3, 6, 2)
    /// </summary>
    /// <param name="count">A quantidade de dados a serem rolados (deve ser maior que zero).</param>
    /// <param name="sides">A quantidade de lados de cada dado (deve ser no mínimo 1).</param>
    /// <param name="modifier">O valor modificador numérico adicionado ao resultado final da rolagem.</param>
    /// <returns>Uma estrutura <see cref="DiceRollResult"/> com o total e o detalhamento individual de cada dado.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Lançada quando <paramref name="count"/> ou <paramref name="sides"/> forem menores que 1.</exception>
    public static DiceRollResult RollDice(int count, int sides, int modifier = 0)
    {
        if (count < 1)
            throw new ArgumentOutOfRangeException(nameof(count),
                "Dice count must be greater than zero.");

        if (sides < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(sides),
                "The dice must have at least 1 side.");
        }

        var rolls = new int[count];
        var sum = 0;

        for (var i = 0; i < count; i++)
        {
            var roll = Roll(sides);
            rolls[i] = roll;
            sum += roll;
        }

        var total = sum + modifier;
        return new DiceRollResult(total, rolls, count, sides, modifier);
    }
    
    /// <summary>
    /// Rola um dado de 3 lados (d3)
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 3.</returns>
    public static int RollD3() => Roll(3);

    /// <summary>
    /// Rola um dado de 4 lados (d4).
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 4.</returns>
    public static int RollD4() => Roll(4);

    /// <summary>
    /// Rola um dado de 6 lados (d6).
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 6.</returns>
    public static int RollD6() => Roll(6);

    /// <summary>
    /// Rola um dado de 8 lados (d8).
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 8.</returns>
    public static int RollD8() => Roll(8);

    /// <summary>
    /// Rola um dado de 10 lados (d10).
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 10.</returns>
    public static int RollD10() => Roll(10);
    
    /// <summary>
    /// Rola um dado de 12 lados (d12)
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 12.</returns>
    public static int RollD12() => Roll(12);

    /// <summary>
    /// Rola um dado de 20 lados (d20).
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 20.</returns>
    public static int RollD20() => Roll(20);

    /// <summary>
    /// Rola um dado percentual de 100 lados (d100).
    /// </summary>
    /// <returns>Um valor inteiro entre 1 e 100.</returns>
    public static int RollD100() => Roll(100);

    /// <summary>
    /// Método clássico para criação de atributos (rola 3d6 e multiplica o total por 5).
    /// </summary>
    /// <returns>O valor final percentual gerado para o atributo.</returns>
    public static int RollAttribute3D6()
    {
        var result = RollDice(3, 6);
        return result.Total * 5;
    }

    /// <summary>
    /// Método para atributos de escala maior (rola 2d6 + 6 e multiplica o total por 5).
    /// </summary>
    /// <returns>O valor final percentual gerado para o atributo.</returns>
    public static int RollAttribute2D6Plus6()
    {
        var result = RollDice(2, 6, modifier: 6);
        return result.Total * 5;
    }
}