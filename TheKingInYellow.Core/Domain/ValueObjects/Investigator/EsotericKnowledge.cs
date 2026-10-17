namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

public record EsotericKnowledge(
    int ForbiddenLoreRating = 0,
    List<string>? ReadForbiddenBooks = null,
    List<string>? KnownSpells = null,
    List<string>? EncounteredEntities = null
);