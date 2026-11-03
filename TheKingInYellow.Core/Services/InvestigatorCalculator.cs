using TheKingInYellow.Core.Domain.Primitives;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Serviço utilitário responsável pelo cálculo de estatísticas derivadas do investigador (HP, Lucidez, MP, Bônus de Dano, Porte e Taxa de Movimento).
/// </summary>
public static class InvestigatorCalculator
{
    /// <summary>
    /// Calcula os Pontos de Vida (HP) máximos do investigador com base em CON e SIZ: (CON + SIZ) / 10.
    /// </summary>
    /// <param name="con">Valor de Constituição (CON).</param>
    /// <param name="siz">Valor de Tamanho (SIZ).</param>
    /// <returns>Um <see cref="Result{T}"/> com o valor máximo de HP calculado.</returns>
    public static Result<int> CalculateMaxHp(int con, int siz)
    {
        if (con < 0 || siz < 0)
            return Result<int>.Failure("Os atributos CON e SIZ não podem ser negativos.", Math.Max(0, (con + siz) / 10));

        return Result<int>.Success((con + siz) / 10);
    }

    /// <summary>
    /// Calcula o valor máximo de Lucidez / Sanidade inicial do investigador com base em POW: igual a POW.
    /// </summary>
    /// <param name="pow">Valor de Poder (POW).</param>
    /// <returns>Um <see cref="Result{T}"/> com o valor máximo de Lucidez calculado.</returns>
    public static Result<int> CalculateMaxLucidity(int pow)
    {
        if (pow < 0)
            return Result<int>.Failure("O atributo POW não pode ser negativo.", Math.Max(0, pow));

        return Result<int>.Success(pow);
    }

    /// <summary>
    /// Calcula os Pontos de Magia (MP) máximos do investigador com base em POW: POW / 5.
    /// </summary>
    /// <param name="pow">Valor de Poder (POW).</param>
    /// <returns>Um <see cref="Result{T}"/> com o valor máximo de MP calculado.</returns>
    public static Result<int> CalculateMaxMp(int pow)
    {
        if (pow < 0)
            return Result<int>.Failure("O atributo POW não pode ser negativo.", Math.Max(0, pow / 5));

        return Result<int>.Success(pow / 5);
    }

    /// <summary>
    /// Determina o Bônus de Dano (Damage Bonus) e o Porte (Build) do investigador com base na soma de STR + SIZ.
    /// </summary>
    /// <param name="str">Valor de Força (STR).</param>
    /// <param name="siz">Valor de Tamanho (SIZ).</param>
    /// <returns>Um <see cref="Result{T}"/> contendo a tupla com a string de bônus de dano e o valor inteiro do porte.</returns>
    public static Result<(string DamageBonus, int Build)> CalculateDamageBonusAndBuild(int str, int siz)
    {
        if (str < 0 || siz < 0)
            return Result<(string DamageBonus, int Build)>.Failure("Os atributos STR e SIZ não podem ser negativos.", ("-2", -2));

        var sum = str + siz;
        (string DamageBonus, int Build) result = sum switch
        {
            <= 64 => ("-2", -2),
            <= 84 => ("-1", -1),
            <= 124 => ("0", 0),
            <= 164 => ("+1d4", 1),
            <= 204 => ("+1d6", 2),
            _ => ("+2d6", 3)
        };

        return Result<(string DamageBonus, int Build)>.Success(result);
    }

    /// <summary>
    /// Calcula a Taxa de Movimento (Movement Rate / MOV) do personagem com base em STR, DEX, SIZ e idade.
    /// </summary>
    /// <param name="str">Valor de Força (STR).</param>
    /// <param name="dex">Valor de Destreza (DEX).</param>
    /// <param name="siz">Valor de Tamanho (SIZ).</param>
    /// <param name="age">Idade do personagem.</param>
    /// <returns>Um <see cref="Result{T}"/> com a taxa de movimento calculada (mínimo de 1).</returns>
    public static Result<int> CalculateMovementRate(int str, int dex, int siz, int age)
    {
        if (str < 0 || dex < 0 || siz < 0 || age < 0)
            return Result<int>.Failure("Atributos e idade não podem ser negativos.", 1);

        int baseMov;

        if (dex < siz && str < siz)
            baseMov = 7;
        else if (str > siz && dex > siz)
            baseMov = 9;
        else
            baseMov = 8;

        if (age < 40) return Result<int>.Success(Math.Max(1, baseMov));

        var decadesAfter40 = (age - 40) / 10;
        var agePenalty = 1 + decadesAfter40;
        baseMov -= agePenalty;

        return Result<int>.Success(Math.Max(1, baseMov));
    }
}