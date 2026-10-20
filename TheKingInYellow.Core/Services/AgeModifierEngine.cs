using TheKingInYellow.Core.Interfaces;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Motor de regras responsável por aplicar modificadores de idade e testes de melhoria educacional em atributos.
/// </summary>
public static class AgeModifierEngine
{
    /// <summary>
    /// Realiza um teste de melhoria de Educação (EDU). Se a rolagem de d100 for maior que o valor atual e maior que 95,
    /// ou falhar no teste de EDU, adiciona 1d10 pontos a EDU (respeitando o teto de 99).
    /// </summary>
    /// <param name="currentEdu">O valor atual de EDU do personagem.</param>
    /// <param name="diceRoller">Instância do rolador de dados.</param>
    /// <returns>O novo valor de EDU após o teste de melhoria.</returns>
    public static int PerformEduImprovementCheck(int currentEdu, IDiceRoller diceRoller)
    {
        var roll = diceRoller.RollD100();

        if (roll <= currentEdu && roll <= 95) return currentEdu;
        var gain = diceRoller.RollD10();
        return Math.Min(99, currentEdu + gain);
    }

    /// <summary>
    /// Aplica ajustes de idade nos atributos do personagem (STR, CON, DEX, APP, EDU, SIZ) de acordo com a faixa etária.
    /// Jovens sofrem reduções em STR e EDU, enquanto personagens mais velhos realizam testes de melhoria de EDU e sofrem deduções físicas e de aparência.
    /// </summary>
    /// <param name="age">A idade do personagem.</param>
    /// <param name="str">Força (STR) inicial.</param>
    /// <param name="con">Constituição (CON) inicial.</param>
    /// <param name="dex">Destreza (DEX) inicial.</param>
    /// <param name="app">Aparência (APP) inicial.</param>
    /// <param name="edu">Educação (EDU) inicial.</param>
    /// <param name="siz">Tamanho (SIZ) inicial.</param>
    /// <param name="diceRoller">Instância do rolador de dados para os testes de melhoria de EDU.</param>
    /// <returns>Tupla contendo os atributos ajustados.</returns>
    public static (int Str, int Con, int Dex, int App, int Edu, int Siz) ApplyAgeAdjustments(
        int age, int str, int con, int dex, int app, int edu, int siz, IDiceRoller diceRoller)
    {
        if (age <= 19)
        {
            str = Math.Max(15, str - 5);
            edu = Math.Max(15, edu - 5);
            return (str, con, dex, app, edu, siz);
        }

        if (age <= 39)
        {
            edu = PerformEduImprovementCheck(edu, diceRoller);
            return (str, con, dex, app, edu, siz);
        }

        int eduChecks = age switch
        {
            <= 49 => 2,
            <= 59 => 3,
            _ => 4
        };

        for (var i = 0; i < eduChecks; i++)
        {
            edu = PerformEduImprovementCheck(edu, diceRoller);
        }

        int physicalDeduction = age switch
        {
            <= 49 => 5,
            <= 59 => 10,
            <= 69 => 20,
            <= 79 => 40,
            _ => 80
        };

        int appDeduction = age switch
        {
            <= 49 => 5,
            <= 59 => 10,
            <= 69 => 15,
            <= 79 => 20,
            _ => 25
        };

        app = Math.Max(15, app - appDeduction);

        while (physicalDeduction > 0)
        {
            if (str >= con && str >= dex && str > 15)
            {
                str -= 5;
            }
            else if (con >= dex && con > 15)
            {
                con -= 5;
            }
            else if (dex > 15)
            {
                dex -= 5;
            }
            else break;

            physicalDeduction -= 5;
        }

        return (str, con, dex, app, edu, siz);
    }
}