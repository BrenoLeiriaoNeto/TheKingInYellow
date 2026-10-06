namespace TheKingInYellow.Core.Domain.ValueObjects;

public record SkillDefinition(
    string Id,
    string Name,
    int BaseValue,
    string Category,
    string Description,
    bool CanBePushed = true,
    string? DerivedAttribute = null
    );