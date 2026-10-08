namespace TheKingInYellow.Core.Domain.Enums;

/// <summary>
/// Define os possíveis níveis de sucesso e falha em um teste de perícia.
/// </summary>
public enum SuccessLevel
{
    /// <summary>
    /// Desastre / Falha Crítica (rolagem 96-100 para perícias &lt; 50, ou 100 para perícias &gt;= 50).
    /// </summary>
    Fumble = 0,

    /// <summary>
    /// Falha normal (rolagem maior que o valor da perícia).
    /// </summary>
    Failure = 1,

    /// <summary>
    /// Sucesso regular / padrão (rolagem menor ou igual ao valor da perícia).
    /// </summary>
    Regular = 2,

    /// <summary>
    /// Sucesso difícil (rolagem menor ou igual à metade do valor da perícia).
    /// </summary>
    Hard = 3,

    /// <summary>
    /// Sucesso extremo (rolagem menor ou igual a um quinto do valor da perícia).
    /// </summary>
    Extreme = 4,

    /// <summary>
    /// Sucesso crítico (rolagem com resultado igual a 1).
    /// </summary>
    Critical = 5
}