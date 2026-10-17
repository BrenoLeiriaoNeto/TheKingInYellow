namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

public record MedicalState(
    bool IsMajorWound = false,
    bool IsTemporarilyInsane = false,
    bool IsIndefinitelyInsane = false,
    List<string>? InsanityEpisodes = null
    );