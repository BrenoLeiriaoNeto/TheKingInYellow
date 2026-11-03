using TheKingInYellow.Core.Domain.ValueObjects;
using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class OccupationEngineTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(40, 80)]
    [InlineData(50, 100)]
    [InlineData(85, 170)]
    public void CalculatePersonalInterestPoints_ValidIntelligence_ReturnsDouble(int intValue, int expectedPoints)
    {
        var result = OccupationEngine.CalculatePersonalInterestPoints(intValue);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedPoints, result.Value);
    }

    [Fact]
    public void CalculatePersonalInterestPoints_NegativeIntelligence_ReturnsFailure()
    {
        var result = OccupationEngine.CalculatePersonalInterestPoints(-10);
        Assert.True(result.IsFailure);
        Assert.Equal("O atributo Inteligência não pode ser negativo.", result.Error);
    }

    [Fact]
    public void CalculateOccupationPoints_PrimaryOnly_ReturnsPrimaryMultiplied()
    {
        var occupation = new OccupationDefinition(
            Id: "professor",
            Name: "Professor",
            Description: "An academic scholar",
            CreditRatingMin: 20,
            CreditRatingMax: 70,
            Formula: new OccupationFormula(
                PrimaryAttr: "EDU",
                PrimaryMultiplier: 4,
                SecondaryChoiceAttrs: [],
                SecondaryMultiplier: 0
            ),
            SkillIds: ["edu_skill"],
            SuggestedContacts: []
        );

        var attributes = new Dictionary<string, int>
        {
            { "EDU", 70 }
        };

        var result = OccupationEngine.CalculateOccupationPoints(occupation, attributes);

        // 70 * 4 = 280
        Assert.True(result.IsSuccess);
        Assert.Equal(280, result.Value);
    }

    [Fact]
    public void CalculateOccupationPoints_WithExplicitSecondaryChoice_CalculatesUsingChosenSecondary()
    {
        var occupation = new OccupationDefinition(
            Id: "detective",
            Name: "Detective",
            Description: "A private investigator",
            CreditRatingMin: 20,
            CreditRatingMax: 50,
            Formula: new OccupationFormula(
                PrimaryAttr: "EDU",
                PrimaryMultiplier: 2,
                SecondaryChoiceAttrs: ["STR", "DEX"],
                SecondaryMultiplier: 2
            ),
            SkillIds: [],
            SuggestedContacts: []
        );

        var attributes = new Dictionary<string, int>
        {
            { "EDU", 60 },
            { "STR", 40 },
            { "DEX", 80 }
        };

        // Explicitly choose STR even though DEX is higher
        var result = OccupationEngine.CalculateOccupationPoints(occupation, attributes, chosenSecondaryAttr: "STR");

        // EDU * 2 + STR * 2 = 60 * 2 + 40 * 2 = 200
        Assert.True(result.IsSuccess);
        Assert.Equal(200, result.Value);
    }

    [Fact]
    public void CalculateOccupationPoints_WithoutSecondaryChoice_AutoSelectsHighestSecondaryAttribute()
    {
        var occupation = new OccupationDefinition(
            Id: "athlete",
            Name: "Athlete",
            Description: "A professional athlete",
            CreditRatingMin: 10,
            CreditRatingMax: 60,
            Formula: new OccupationFormula(
                PrimaryAttr: "EDU",
                PrimaryMultiplier: 2,
                SecondaryChoiceAttrs: ["STR", "DEX", "CON"],
                SecondaryMultiplier: 2
            ),
            SkillIds: [],
            SuggestedContacts: []
        );

        var attributes = new Dictionary<string, int>
        {
            { "EDU", 50 },
            { "STR", 75 },
            { "DEX", 90 },
            { "CON", 60 }
        };

        // Null secondary choice should auto-select DEX (90)
        var result = OccupationEngine.CalculateOccupationPoints(occupation, attributes, chosenSecondaryAttr: null);

        // EDU * 2 + DEX * 2 = 50 * 2 + 90 * 2 = 280
        Assert.True(result.IsSuccess);
        Assert.Equal(280, result.Value);
    }

    [Fact]
    public void CalculateOccupationPoints_WithInvalidSecondaryChoice_FallsBackToHighestSecondaryAttribute()
    {
        var occupation = new OccupationDefinition(
            Id: "athlete",
            Name: "Athlete",
            Description: "A professional athlete",
            CreditRatingMin: 10,
            CreditRatingMax: 60,
            Formula: new OccupationFormula(
                PrimaryAttr: "EDU",
                PrimaryMultiplier: 2,
                SecondaryChoiceAttrs: ["STR", "DEX"],
                SecondaryMultiplier: 2
            ),
            SkillIds: [],
            SuggestedContacts: []
        );

        var attributes = new Dictionary<string, int>
        {
            { "EDU", 50 },
            { "STR", 70 },
            { "DEX", 85 }
        };

        // "POW" is not in SecondaryChoiceAttrs -> should fallback to highest (DEX: 85)
        var result = OccupationEngine.CalculateOccupationPoints(occupation, attributes, chosenSecondaryAttr: "POW");

        // EDU * 2 + DEX * 2 = 50 * 2 + 85 * 2 = 270
        Assert.True(result.IsSuccess);
        Assert.Equal(270, result.Value);
    }

    [Fact]
    public void CalculateOccupationPoints_MissingAttributeInDictionary_TreatsAsZero()
    {
        var occupation = new OccupationDefinition(
            Id: "scholar",
            Name: "Scholar",
            Description: "A researcher",
            CreditRatingMin: 10,
            CreditRatingMax: 40,
            Formula: new OccupationFormula(
                PrimaryAttr: "EDU",
                PrimaryMultiplier: 4,
                SecondaryChoiceAttrs: ["INT"],
                SecondaryMultiplier: 2
            ),
            SkillIds: [],
            SuggestedContacts: []
        );

        // Dictionary is empty
        var attributes = new Dictionary<string, int>();

        var result = OccupationEngine.CalculateOccupationPoints(occupation, attributes);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value);
    }

    [Fact]
    public void CalculateOccupationPoints_NullOccupation_ReturnsFailure()
    {
        var attributes = new Dictionary<string, int> { { "EDU", 60 } };
        var result = OccupationEngine.CalculateOccupationPoints(null!, attributes);

        Assert.True(result.IsFailure);
        Assert.Equal("A definição da ocupação não pode ser nula.", result.Error);
    }

    [Fact]
    public void CalculateOccupationPoints_NullAttributes_ReturnsFailure()
    {
        var occupation = new OccupationDefinition(
            Id: "scholar",
            Name: "Scholar",
            Description: "A researcher",
            CreditRatingMin: 10,
            CreditRatingMax: 40,
            Formula: new OccupationFormula("EDU", 4, [], 0),
            SkillIds: [],
            SuggestedContacts: []
        );

        var result = OccupationEngine.CalculateOccupationPoints(occupation, null!);

        Assert.True(result.IsFailure);
        Assert.Equal("O dicionário de atributos não pode ser nulo.", result.Error);
    }
}
