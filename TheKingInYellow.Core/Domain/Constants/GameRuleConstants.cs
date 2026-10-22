namespace TheKingInYellow.Core.Domain.Constants;

/// <summary>
/// Contém constantes globais e regras de casa do sistema de jogo.
/// </summary>
public static class GameRuleConstants
{
    /// <summary>
    /// Regra de Casa: Teto máximo de investimento em uma perícia durante a criação do investigador.
    /// </summary>
    public const int MaxInitialSkillValue = 90;

    /// <summary>
    /// Regra de Casa: Teto máximo de qualquer perícia durante os testes de melhorias de descanso.
    /// </summary>
    public const int MaxAbsoluteSkillValue = 99;
}