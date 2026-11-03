using TheKingInYellow.Core.Domain.Primitives;
using TheKingInYellow.Core.Domain.ValueObjects;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Serviço estático responsável por resolver tokens, grupos e opções de escolha de perícias em ocupações.
/// </summary>
public static class OccupationSlotResolver
{
    /// <summary>
    /// Mapeamento centralizado de tokens de grupo do JSON para as respectivas tags de grupo de perícias.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> TokenToGroupMap = new Dictionary<string, string>
    {
        { "CHOICE_INTERPERSONAL", "interpersonal" },
        { "CHOICE_SCIENCE", "science" },
        { "CHOICE_ART_AND_LABOR", "art_and_labor" },
        { "CHOICE_LANGUAGES", "languages" }
    };

    /// <summary>
    /// Verifica se o token informado representa um slot curinga ou de escolha múltipla (wildcard) de perícia.
    /// </summary>
    /// <param name="skillIdToken">O token ou identificador a ser validado.</param>
    /// <returns><c>true</c> se o token representar uma escolha dinâmica (ex: ANY, CHOICE_*, CHOICE:...); caso contrário, <c>false</c>.</returns>
    public static bool IsWildcardToken(string skillIdToken)
    {
        if (string.IsNullOrEmpty(skillIdToken)) return false;

        return skillIdToken.StartsWith("CHOICE_") ||
               skillIdToken.StartsWith("CHOICE:") ||
               skillIdToken == "ANY";
    }

    /// <summary>
    /// Retorna a lista de identificadores de perícias válidas para preencher o slot informado.
    /// Usado pela interface gráfica no Godot para popular os menus de seleção (Dropdowns/Comboboxes) do jogador.
    /// </summary>
    /// <param name="skillIdToken">O token que define o slot de perícia (identificador direto, token de grupo ou token de escolha).</param>
    /// <param name="allSkillDefinitions">A lista completa de todas as definições de perícias disponíveis.</param>
    /// <returns>Um <see cref="Result{T}"/> contendo a lista de identificadores elegíveis em caso de sucesso, ou mensagem de erro em caso de falha.</returns>
    public static Result<IReadOnlyList<string>> GetValidOptionsForToken(
        string skillIdToken,
        IReadOnlyList<SkillDefinition> allSkillDefinitions)
    {
        if (string.IsNullOrWhiteSpace(skillIdToken))
            return Result<IReadOnlyList<string>>.Failure("O token de perícia não pode ser nulo ou vazio.", Array.Empty<string>());

        if (allSkillDefinitions == null)
            return Result<IReadOnlyList<string>>.Failure("A lista de definições de perícias não pode ser nula.", Array.Empty<string>());

        if (skillIdToken.StartsWith("CHOICE:"))
        {
            var options = skillIdToken["CHOICE:".Length..].Split('|');
            return Result<IReadOnlyList<string>>.Success(options);
        }

        if (skillIdToken == "ANY")
        {
            return Result<IReadOnlyList<string>>.Success(allSkillDefinitions.Select(s => s.Id).ToList());
        }

        if (TokenToGroupMap.TryGetValue(skillIdToken, out var groupName))
        {
            return Result<IReadOnlyList<string>>.Success(allSkillDefinitions
                .Where(s => s.Group == groupName)
                .Select(s => s.Id)
                .ToList());
        }

        return Result<IReadOnlyList<string>>.Success([skillIdToken]);
    }
}