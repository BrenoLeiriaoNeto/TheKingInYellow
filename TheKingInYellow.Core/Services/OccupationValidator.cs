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
    /// <returns><c>true</c> se a seleção de perícias for válida para a ocupação informada; caso contrário, <c>false</c>.</returns>
    public static bool ValidateSkillSelection(
        OccupationDefinition occupation,
        IReadOnlyList<string> chosenSkillIds,
        IReadOnlyList<SkillDefinition> allSkillDefinitions)
    {
        if (chosenSkillIds.Count != 8 || chosenSkillIds.Distinct().Count() != 8) 
            return false;

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

        return unassignedChosen.Count == 0;
    }
}