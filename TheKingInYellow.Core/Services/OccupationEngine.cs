using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Serviço estático responsável pelo cálculo de pontos de perícia ocupacionais e de interesse pessoal.
/// </summary>
public static class OccupationEngine
{
    /// <summary>
    /// Calcula os Pontos de Interesse Pessoais disponíveis para o personagem (geralmente INT * 2),
    /// os quais podem ser distribuídos livremente em qualquer perícia do jogo.
    /// </summary>
    /// <param name="intelligence">O valor do atributo Inteligência (INT) do personagem.</param>
    /// <returns>A quantidade total de pontos de interesse pessoal disponíveis.</returns>
    public static int CalculatePersonalInterestPoints(int intelligence) => intelligence * 2;

    /// <summary>
    /// Calcula o total de Pontos Ocupacionais da profissão escolhida com base nos atributos do personagem.
    /// Se a profissão oferecer opções de atributo secundário (ex: DEX ou STR) e nenhum for informado explicitamente,
    /// seleciona automaticamente o atributo de maior valor do personagem.
    /// </summary>
    /// <param name="occupation">A definição da ocupação contendo a fórmula de cálculo.</param>
    /// <param name="attributes">Dicionário contendo os atributos do personagem e seus respectivos valores.</param>
    /// <param name="chosenSecondaryAttr">O atributo secundário explicitamente escolhido pelo jogador, se aplicável.</param>
    /// <returns>O total de pontos de perícia ocupacionais calculados.</returns>
    public static int CalculateOccupationPoints(
        OccupationDefinition occupation,
        IReadOnlyDictionary<string, int> attributes,
        string? chosenSecondaryAttr = null)
    {
        var primaryVal = attributes.GetValueOrDefault(occupation.Formula.PrimaryAttr, 0);
        var totalPoints = primaryVal * occupation.Formula.PrimaryMultiplier;

        if (occupation.Formula is not
            { SecondaryMultiplier: > 0, SecondaryChoiceAttrs.Count: > 0 }) return totalPoints;
        string selectedAttr;

        if (!string.IsNullOrEmpty(chosenSecondaryAttr) &&
            occupation.Formula.SecondaryChoiceAttrs.Contains(chosenSecondaryAttr))
        {
            selectedAttr = chosenSecondaryAttr;
        }
        else
        {
            selectedAttr = occupation.Formula.SecondaryChoiceAttrs
                .OrderByDescending(attr => attributes.GetValueOrDefault(attr, 0))
                .First();
        }

        var secondaryVal = attributes.GetValueOrDefault(selectedAttr, 0);
        totalPoints += secondaryVal * occupation.Formula.SecondaryMultiplier;

        return totalPoints;
    }
}