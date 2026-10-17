namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

public record Finances(
    decimal CashOnHand,
    decimal SpendingLevel,
    decimal Assets,
    List<string>? RealEstateAndPossessions = null
    );