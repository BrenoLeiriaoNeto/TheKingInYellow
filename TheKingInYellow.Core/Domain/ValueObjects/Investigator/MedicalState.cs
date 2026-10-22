namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

/// <summary>
/// Representa a condição médica, saúde mental e física do investigador.
/// </summary>
/// <param name="IsMajorWound">Indica se o investigador sofreu uma lesão grave recente.</param>
/// <param name="IsTemporarilyInsane">Indica se o investigador está em estado de loucura temporária.</param>
/// <param name="IsIndefinitelyInsane">Indica se o investigador desenvolveu loucura indefinida.</param>
/// <param name="InsanityEpisodes">Lista com o histórico de episódios de insanidade vivenciados.</param>
public record MedicalState(
    bool IsMajorWound = false,
    bool IsTemporarilyInsane = false,
    bool IsIndefinitelyInsane = false,
    List<string>? InsanityEpisodes = null
);