namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

/// <summary>
/// Representa o conhecimento esotérico, ocultismo e encontros sobrenaturais do investigador.
/// </summary>
/// <param name="ForbiddenLoreRating">O nível/pontuação de conhecimento proibido acumulado.</param>
/// <param name="ReadForbiddenBooks">Lista de tomos e livros proibidos lidos pelo investigador.</param>
/// <param name="KnownSpells">Lista de feitiços e rituais conhecidos.</param>
/// <param name="EncounteredEntities">Lista de entidades cósmicas e criaturas sobrenaturais encontradas.</param>
public record EsotericKnowledge(
    int ForbiddenLoreRating = 0,
    List<string>? ReadForbiddenBooks = null,
    List<string>? KnownSpells = null,
    List<string>? EncounteredEntities = null
);