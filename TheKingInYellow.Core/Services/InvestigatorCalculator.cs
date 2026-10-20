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
    /// <returns>O valor máximo de HP.</returns>
    public static int CalculateMaxHp(int con, int siz) => (con + siz) / 10;

    /// <summary>
    /// Calcula o valor máximo de Lucidez / Sanidade inicial do investigador com base em POW: igual a POW.
    /// </summary>
    /// <param name="pow">Valor de Poder (POW).</param>
    /// <returns>O valor máximo de Lucidez.</returns>
    public static int CalculateMaxLucidity(int pow) => pow;

    /// <summary>
    /// Calcula os Pontos de Magia (MP) máximos do investigador com base em POW: POW / 5.
    /// </summary>
    /// <param name="pow">Valor de Poder (POW).</param>
    /// <returns>O valor máximo de MP.</returns>
    public static int CalculateMaxMp(int pow) => pow / 5;

    /// <summary>
    /// Determina o Bônus de Dano (Damage Bonus) e o Porte (Build) do investigador com base na soma de STR + SIZ.
    /// </summary>
    /// <param name="str">Valor de Força (STR).</param>
    /// <param name="siz">Valor de Tamanho (SIZ).</param>
    /// <returns>Tupla contendo a string de bônus de dano e o valor inteiro do porte.</returns>
    public static (string DamageBonus, int Build) CalculateDamageBonusAndBuild(int str, int siz)
    {
        var sum = str + siz;
        return sum switch
        {
            <= 64 => ("-2", -2),
            <= 84 => ("-1", -1),
            <= 124 => ("0", 0),
            <= 164 => ("+1d4", 1),
            <= 204 => ("+1d6", 2),
            _ => ("+2d6", 3)
        };
    }

    /// <summary>
    /// Calcula a Taxa de Movimento (Movement Rate / MOV) do personagem com base em STR, DEX, SIZ e idade.
    /// </summary>
    /// <param name="str">Valor de Força (STR).</param>
    /// <param name="dex">Valor de Destreza (DEX).</param>
    /// <param name="siz">Valor de Tamanho (SIZ).</param>
    /// <param name="age">A idade do personagem.</param>
    /// <returns>A taxa de movimento calculada (mínimo de 1).</returns>
    public static int CalculateMovementRate(int str, int dex, int siz, int age)
    {
        int baseMov;

        if (dex < siz && str < siz)
            baseMov = 7;
        else if (str > siz && dex > siz)
            baseMov = 9;
        else
            baseMov = 8;

        if (age < 40) return Math.Max(1, baseMov);

        var decadesAfter40 = (age - 40) / 10;
        var agePenalty = 1 + decadesAfter40;
        baseMov -= agePenalty;

        return Math.Max(1, baseMov);
    }
}