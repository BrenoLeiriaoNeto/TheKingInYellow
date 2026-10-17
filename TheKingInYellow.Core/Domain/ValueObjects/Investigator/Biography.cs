namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

public record Biography(
    string Residence = "",
    string PersonalDescription = "",
    string IdeologyBeliefs = "",
    string SignificantPeople = "",
    string MeaningfulLocations = "",
    List<string>? ScarsAndInjuries = null,
    List<string>? PhobiasAndManias = null
    );