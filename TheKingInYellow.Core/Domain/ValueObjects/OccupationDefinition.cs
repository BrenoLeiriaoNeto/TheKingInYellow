namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Representa a fórmula matemática para o cálculo dos pontos de perícia ocupacionais de uma profissão.
/// </summary>
/// <param name="PrimaryAttr">O identificador do atributo principal utilizado no cálculo dos pontos ocupacionais.</param>
/// <param name="PrimaryMultiplier">O multiplicador aplicado ao valor do atributo principal.</param>
/// <param name="SecondaryChoiceAttrs">A lista de identificadores dos atributos opcionais disponíveis para escolha como secundário.</param>
/// <param name="SecondaryMultiplier">O multiplicador aplicado ao valor do atributo secundário escolhido.</param>
public record OccupationFormula(
    string PrimaryAttr,
    int PrimaryMultiplier,
    IReadOnlyList<string> SecondaryChoiceAttrs,
    int SecondaryMultiplier
    );

/// <summary>
/// Representa a definição completa de uma ocupação ou profissão no jogo.
/// </summary>
/// <param name="Id">O identificador único da ocupação.</param>
/// <param name="Name">O nome da ocupação.</param>
/// <param name="Description">A descrição temática e detalhes da ocupação.</param>
/// <param name="CreditRatingMin">O valor mínimo permitido para a perícia de Nível de Crédito.</param>
/// <param name="CreditRatingMax">O valor máximo permitido para a perícia de Nível de Crédito.</param>
/// <param name="Formula">A fórmula utilizada para o cálculo dos pontos de perícia ocupacionais.</param>
/// <param name="SkillIds">A lista de identificadores ou tokens de perícias obrigatórias e de escolha da ocupação.</param>
/// <param name="SuggestedContacts">A lista de descrições ou sugestões de contatos para o personagem.</param>
public record OccupationDefinition(
    string Id,
    string Name,
    string Description,
    int CreditRatingMin,
    int CreditRatingMax,
    OccupationFormula Formula,
    IReadOnlyList<string> SkillIds,
    IReadOnlyList<string> SuggestedContacts);