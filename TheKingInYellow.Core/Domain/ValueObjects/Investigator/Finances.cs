namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

/// <summary>
/// Representa a situação financeira, patrimônio e posses do investigador.
/// </summary>
/// <param name="CashOnHand">O montante de dinheiro disponível em espécie para gastos imediatos.</param>
/// <param name="SpendingLevel">O limite diário de gastos razoáveis sem necessidade de justificar patrimônio.</param>
/// <param name="Assets">O valor total de bens e patrimônio líquido acumulado.</param>
/// <param name="RealEstateAndPossessions">Lista descritiva de propriedades imobiliárias e posses relevantes.</param>
public record Finances(
    decimal CashOnHand,
    decimal SpendingLevel,
    decimal Assets,
    List<string>? RealEstateAndPossessions = null
);