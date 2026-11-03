using TheKingInYellow.Core.Domain.Primitives;
using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Serviço estático responsável por validar a seleção de perícias de um personagem de acordo com as regras da ocupação.
/// </summary>
public static class OccupationValidator
{
    /// <summary>
    /// Valida as perícias selecionadas em relação aos requisitos e restrições de uma determinada ocupação.
    /// Garante que as perícias escolhidas correspondam aos identificadores específicos, grupos e slots personalizáveis
    /// definidos na configuração da ocupação.
    /// </summary>
    /// <param name="occupation">A definição da ocupação contendo as restrições e slots de perícias exigidos.</param>
    /// <param name="chosenSkillIds">A lista de identificadores das perícias selecionadas pelo jogador.</param>
    /// <param name="allSkillDefinitions">A lista completa contendo todas as definições de perícias disponíveis.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha com a descrição da inconsistência encontrada.</returns>
    public static Result ValidateSkillSelection(
        OccupationDefinition occupation,
        IReadOnlyList<string> chosenSkillIds,
        IReadOnlyList<SkillDefinition> allSkillDefinitions)
    {
        if (occupation == null)
            return Result.Failure("A definição da ocupação não pode ser nula.");

        if (chosenSkillIds == null || chosenSkillIds.Count != 8)
            return Result.Failure("A seleção de perícias deve conter exatamente 8 perícias.");

        if (chosenSkillIds.Distinct().Count() != 8)
            return Result.Failure("A seleção de perícias não pode conter perícias duplicadas.");

        if (allSkillDefinitions == null)
            return Result.Failure("A lista de definições de perícias não pode ser nula.");

        var skillsMap = allSkillDefinitions.ToDictionary(s => s.Id);
        var remainingSlots = occupation.SkillIds.ToList();
        var unassignedChosen = chosenSkillIds.ToList();

        for (var i = unassignedChosen.Count - 1; i >= 0; i--)
        {
            var chosenId = unassignedChosen[i];
            if (remainingSlots.Remove(chosenId))
            {
                unassignedChosen.RemoveAt(i);
            }
        }

        for (var i = unassignedChosen.Count - 1; i >= 0; i--)
        {
            var chosenId = unassignedChosen[i];

            var matchingChoiceToken = remainingSlots.FirstOrDefault(token =>
                token.StartsWith("CHOICE:") &&
                token["CHOICE:".Length..].Split('|').Contains(chosenId));

            if (matchingChoiceToken == null) continue;

            remainingSlots.Remove(matchingChoiceToken);
            unassignedChosen.RemoveAt(i);
        }

        for (var i = unassignedChosen.Count - 1; i >= 0; i--)
        {
            var chosenId = unassignedChosen[i];
            if (!skillsMap.TryGetValue(chosenId, out var skillDef) || skillDef.Group == null)
                continue;

            var matchingToken = remainingSlots.FirstOrDefault(token =>
                OccupationSlotResolver.TokenToGroupMap.TryGetValue(token, out var groupName) &&
                groupName == skillDef.Group);

            if (matchingToken == null) continue;

            remainingSlots.Remove(matchingToken);
            unassignedChosen.RemoveAt(i);
        }

        for (var i = unassignedChosen.Count - 1; i >= 0; i--)
        {
            if (remainingSlots.Remove("ANY"))
            {
                unassignedChosen.RemoveAt(i);
            }
        }

        if (unassignedChosen.Count > 0)
        {
            return Result.Failure(
                $"As seguintes perícias não atendem aos requisitos da ocupação: {string.Join(", ", unassignedChosen)}.");
        }

        return Result.Success();
    }
}