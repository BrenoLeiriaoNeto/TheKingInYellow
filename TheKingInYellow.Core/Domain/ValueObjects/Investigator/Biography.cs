namespace TheKingInYellow.Core.Domain.ValueObjects.Investigator;

/// <summary>
/// Representa as informações biográficas e histórico pessoal do investigador.
/// </summary>
/// <param name="Residence">O local de residência atual do investigador.</param>
/// <param name="PersonalDescription">A descrição visual e comportamental do personagem.</param>
/// <param name="IdeologyBeliefs">Ideologias, crenças e filosofia de vida do investigador.</param>
/// <param name="SignificantPeople">Pessoas de grande importância na vida do investigador.</param>
/// <param name="MeaningfulLocations">Locais de valor sentimental ou histórico para o personagem.</param>
/// <param name="ScarsAndInjuries">Lista de cicatrizes, lesões e marcas físicas permanentes.</param>
/// <param name="PhobiasAndManias">Lista de fobias, manias e distúrbios adquiridos.</param>
public record Biography(
    string Residence = "",
    string PersonalDescription = "",
    string IdeologyBeliefs = "",
    string SignificantPeople = "",
    string MeaningfulLocations = "",
    List<string>? ScarsAndInjuries = null,
    List<string>? PhobiasAndManias = null
);