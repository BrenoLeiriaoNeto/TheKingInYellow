using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

public static class DiceRoller
{
    /// <summary>
    /// Rola um único dado com a quantidade de lados especificada.
    /// </summary>
    /// <param name="sides"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static int Roll(int sides)
    {
        if (sides < 1)
            throw new ArgumentOutOfRangeException(nameof(sides),
                "The dice must have at least 1 side.");

        return Random.Shared.Next(1, sides + 1);
    }

    /// <summary>
    /// Rola multiplos dados do mesmo tipo e aplica um modificador ao total.
    /// Exemplo: 3d6 + 2 -> RollDice(3, 6, 2)
    /// </summary>
    /// <param name="count"></param>
    /// <param name="sides"></param>
    /// <param name="modifier"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    
    public static int RollD4() => Roll(4);
    public static int RollD6() => Roll(6);
    public static int RollD8() => Roll(8);
    public static int RollD10() => Roll(10);
    public static int RollD20() => Roll(20);
    public static int RollD100() => Roll(100);

    /// <summary>
    /// Método clássico para criação de atributos
    /// </summary>
    /// <returns></returns>
    public static int RollAttribute3D6()
    {
        var result = RollDice(3, 6);
        return result.Total * 5;
    }

    /// <summary>
    /// Método para atributos de escala maior
    /// </summary>
    /// <returns></returns>
    public static int RollAttribute2D6Plus6()
    {
        var result = RollDice(2, 6, modifier: 6);
        return result.Total * 5;
    }
}