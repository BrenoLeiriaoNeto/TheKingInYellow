namespace TheKingInYellow.Core.Domain.ValueObjects;

/// <summary>
/// Representa a definição base e configuração de uma perícia no sistema.
/// </summary>
/// <param name="Id">O identificador único da perícia.</param>
/// <param name="Name">O nome de exibição da perícia.</param>
/// <param name="BaseValue">O valor percentual base padrão da perícia.</param>
/// <param name="Category">A categoria à qual a perícia pertence (ex: Combate, Acadêmica, Social).</param>
/// <param name="Description">A descrição textual das aplicações e regras da perícia.</param>
/// <param name="CanBePushed">Indica se o teste desta perícia pode ser forçado (Push) em caso de falha inicial.</param>
/// <param name="DerivedAttribute">Identificador de cálculo dinâmico de valor base derivado de atributo (ex: "DEX_DIV_2"), ou nulo se possuir valor fixo.</param>
/// <param name="Group">O grupo temático da perícia para resolução de slots de ocupação (ex: "interpersonal", "science"), ou nulo se não agrupada.</param>
public record SkillDefinition(
    string Id,
    string Name,
    int BaseValue,
    string Category,
    string Description,
    bool CanBePushed = true,
    string? DerivedAttribute = null,
    string? Group = null
    );